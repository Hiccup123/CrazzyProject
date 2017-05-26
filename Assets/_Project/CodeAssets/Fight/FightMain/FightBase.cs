using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

namespace Fight
{
    public abstract class FightBase
    {
        protected bool m_excute_end;

        protected FightMain m_main;

        public FightAction m_next_action;

        public FightBase(FightMain p_data)
        {
            m_main = p_data;
        }

        //执行当前状态
        public virtual void OnExcute()
        {
            m_excute_end = false;
        }

        public virtual void InputUpdate()
        {

        }

        //检测当前状态是否完成
        public virtual FightStatus OnUpdate()
        {
            if (m_excute_end)
            {
                return FightStatus.Finish;
            }
            return FightStatus.Running;
        }

        //转换下一状态
        public abstract FightAction TurnNextAction();
    }

    //取消连线
    public class FightCancelLine : FightBase
    {
        public FightCancelLine(FightMain p_data) : base(p_data)
        {

        }

        public override void OnExcute()
        {
            base.OnExcute();
            foreach (var t_item in m_main.Dispose_List)
            {
                FightSingle t_single = t_item as FightSingle;
                //t_single.GetIcon.color = Color.white;
                t_single.GetIcon.TweenScale(Vector3.one, 0.1f);
            }
            m_main.Temp_Obj = null;
            m_main.Dispose_List.Clear();
            m_main.CleanArrow();
            m_excute_end = true;
        }

        public override FightAction TurnNextAction()
        {
            return FightAction.Line;
        }
    }

}

