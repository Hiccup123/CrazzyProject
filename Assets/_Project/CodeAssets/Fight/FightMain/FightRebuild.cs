using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

namespace Fight
{
    public class FightRebuild : FightBase
    {

        private int m_cur_dis_count;

        public FightRebuild(FightMain p_data) : base(p_data)
        {

        }

        public override void OnExcute()
        {
            m_cur_dis_count = 0;
            base.OnExcute();
            Rebuild();
        }

        //重置item
        void Rebuild()
        {
            for (int i = 0; i < m_main.Fight_List.GetLength(0); i++)
            {
                for (int j = 0; j < m_main.Fight_List.GetLength(1); j++)
                {
                    FightSingle t_single = m_main.Fight_List[i, j] as FightSingle;
                    if (!string.IsNullOrEmpty(t_single.GetIcon.icon))
                    {
                        m_main.Dispose_List.Add(m_main.Fight_List[i, j]);
                    }
                }
            }

            m_main.RemoveItem(true, CheckDisCount);
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

        //void Rebuild()
        //{
        //    int t_count = 0;
        //    for (int i = 0; i < m_fight_items.GetLength(0); i++)
        //    {
        //        for (int j = 0; j <= m_fight_items.GetLength(1) / 2 - 1; j++)
        //        {
        //            GObject t_obj_1 = m_fight_items[i, j];      //交换对象A
        //            GObject t_obj_2 = m_fight_items[i, j + 1 + m_fight_items.GetLength(1) / 2];  //交换对象B
        //            FightSingle t_single = m_fight_items[i, j] as FightSingle;
        //            FightSingle t_single_turn = m_fight_items[i, j + 1 + m_fight_items.GetLength(1) / 2] as FightSingle;

        //            BoxIndex t_temp_box_index = t_single.Box_Index;
        //            t_single.Box_Index = t_single_turn.Box_Index;
        //            t_single_turn.Box_Index = t_temp_box_index;

        //            Vector3 t_pos_1 = t_obj_1.position;     //A对象的位置
        //            Vector3 t_pos_2 = t_obj_2.position;     //B对象的位置

        //            //t_single.InItMove(t_pos_2);
        //            //t_single_turn.InItMove(t_pos_1);

        //            Ease t_type = Ease.Linear;
        //            Tweener t_tweener_1 = m_fight_items[i, j].TweenMove(t_pos_2, m_speed * 5);
        //            t_tweener_1.SetEase(t_type);

        //            Tweener t_tweener_2 = m_fight_items[i, j + 1 + m_fight_items.GetLength(1) / 2].TweenMove(t_pos_1, m_speed * 5);
        //            t_tweener_2.SetEase(t_type);

        //            t_tweener_1.OnComplete(() =>
        //            {
        //                t_count++;
        //                //Debug.Log("t_count1:" + t_count);
        //            });
        //            t_tweener_2.OnComplete(() =>
        //            {
        //                t_count++;
        //                //Debug.Log("t_count2:" + t_count);
        //                if (t_count == (m_fight_items.GetLength(1) - 1) * m_fight_items.GetLength(0))
        //                {
        //                    Debug.Log("Finished");
        //                    CheckAble();
        //                }
        //            });
        //            //Debug.Log("fade");

        //            m_fight_items[i, j] = t_obj_2;
        //            m_fight_items[i, j + 1 + m_fight_items.GetLength(1) / 2] = t_obj_1;  //调换A、B对象之间的位置，完成下落
        //        }
        //    }
        //}
    }
}

