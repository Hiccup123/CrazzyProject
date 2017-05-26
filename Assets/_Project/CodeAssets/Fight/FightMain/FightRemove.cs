using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

namespace Fight
{
    public class FightRemove : FightBase
    {

        private int m_cur_dis_count;

        public FightRemove(FightMain p_data) : base(p_data)
        {

        }

        public override void OnExcute()
        {
            m_cur_dis_count = 0;
            base.OnExcute();
            m_main.RemoveItem(false, CheckDisCount);
        }

        //消除后释放动作
        void CheckDisCount()
        {
            //Debug.Log("m_cur_dispose_count:" + m_cur_dispose_count);
            m_cur_dis_count++;
            if (m_cur_dis_count == m_main.Dispose_List.Count)
            {
                m_excute_end = true;
            }
        }

        public override FightAction TurnNextAction()
        {
            return FightAction.Repair;
        }
    }
}

