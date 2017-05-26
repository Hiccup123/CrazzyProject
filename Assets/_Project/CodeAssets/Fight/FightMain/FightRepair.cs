using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

namespace Fight
{
    public class FightRepair : FightBase
    {

        public FightRepair(FightMain p_data) : base(p_data)
        {

        }

        public override void OnExcute()
        {
            base.OnExcute();
            Repair();
        }

        //补全消除的item
        void Repair()
        {
            int t_count = 0;
            int t_temp = 0;
            bool t_flag = false;

            for (int i = 0; i < m_main.Fight_List.GetLength(1); i++)
            {
                for (int j = m_main.Fight_List.GetLength(0) - 1; j >= 0; j--)
                {
                    FightSingle t_single = m_main.Fight_List[j, i] as FightSingle;

                    if (t_single.Choose)
                    {
                        //t_single.GetIcon.color = Color.white;

                        t_flag = true;
                        if (j >= 1)
                        {
                            t_count++;

                            GObject t_obj_1 = m_main.Fight_List[j, i];  //被消除的A对象
                            GObject t_obj_2 = m_main.Fight_List[j - 1, i];  //A对象上面的一个B对象
                            Vector3 t_pos_1 = t_obj_1.position;     //A对象的位置
                            Vector3 t_pos_2 = t_obj_2.position;     //B对象的位置
                            Tweener t_tweener = m_main.Fight_List[j - 1, i].TweenMove(t_pos_1, m_main.Speed);
                            t_tweener.SetEase(Ease.OutBack);
                            //t_tweener.SetUpdate(true);
                            m_main.Fight_List[j, i].position = t_pos_2;
                            t_tweener.OnComplete(() =>
                            {
                                //FightSingle t_scale_single = m_main.Fight_List[j - 1, i] as FightSingle;
                                //Tweener t_scale = t_scale_single.TweenScaleY(0.8f, 0.1f);
                                //t_scale.OnComplete(() =>
                                //{
                                //    t_scale_single.TweenScaleY(1, 0.1f);
                                //});

                                t_temp++;
                                
                                if (t_temp == t_count)
                                {
                                    if (t_flag)
                                    {
                                        Repair();
                                    }
                                }
                            });

                            //调换A、B对象之间的位置，完成下落
                            FightSingle t_single2 = m_main.Fight_List[j - 1, i] as FightSingle;
                            m_main.Fight_List[j, i] = t_obj_2;
                            m_main.Fight_List[j - 1, i] = t_obj_1;
                            //调换A、B对象的下标
                            BoxIndex t_temp_box_index = t_single.Box_Index;
                            t_single.Box_Index = t_single2.Box_Index;
                            t_single2.Box_Index = t_temp_box_index;
                        }
                        else
                        {
                            Tweener t_tweener = t_single.GetIcon.TweenScale(Vector3.one, m_main.Speed * 2);
                            t_single.SetIcon(m_main.GetRandomId());
                            t_single.Choose = false;
                            t_single.Box_Index = new BoxIndex(0, i);
                            t_tweener.OnComplete(() =>
                            {
                                if (t_count == 0)
                                {
                                    m_excute_end = true;
                                }
                            });
                        }
                    }
                }
            }

            if (!t_flag)
            {
                m_excute_end = true;
            }
        }

        public override FightAction TurnNextAction()
        {
            return FightAction.Check_Comb;
        }
    }
}

