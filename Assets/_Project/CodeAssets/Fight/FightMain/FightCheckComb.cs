using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

namespace Fight
{
    public class FightCheckComb : FightBase
    {

        public FightCheckComb(FightMain p_data) : base(p_data)
        {

        }

        public override void OnExcute()
        {
            base.OnExcute();
            CheckAble();
        }

        void CheckAble()
        {
            m_main.Comb_List.Clear();

            int t_max_x = m_main.Fight_List.GetLength(0);
            int t_max_y = m_main.Fight_List.GetLength(1);

            for (int i = 0; i < t_max_x; i++)
            {
                for (int j = 0; j < t_max_y; j++)
                {
                    FightSingle t_single = m_main.Fight_List[i, j] as FightSingle;
                    t_single.HaveCheck = false;
                    t_single.MatchCount = 0;
                }
            }

            for (int i = 0; i < t_max_x; i++)
            {
                for (int j = 0; j < t_max_y; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        FightSingle t_first = m_main.Fight_List[i, j] as FightSingle;
                        t_first.HaveCheck = true;
                    }

                    List<GObject> m_can_list = new List<GObject>();
                    GetAbleList(m_main.Fight_List[i, j], m_can_list);
                    //Debug.Log("m_can_list:" + m_can_list.Count);

                    if (m_can_list.Count >= 3)
                    {
                        if (m_can_list.Count > 3)
                        {
                            MatchItem(m_can_list);
                        }
                        m_main.Comb_List.Add(m_can_list);
                        //for (int m = 0; m < m_can_list.Count; m++)
                        //{
                        //    FightSingle t_single = m_can_list[m] as FightSingle;
                        //    t_single.GetIcon.alpha = 0.5f;
                        //}
                    }
                }
            }
            //Debug.Log("m_tip_list.Count:" + m_main.Comb_List.Count);

            m_main.Dispose_List.Clear();
            m_main.Temp_Obj = null;

            m_excute_end = true;
        }

        //得到可能的list
        void GetAbleList(GObject p_obj, List<GObject> p_list)
        {
            FightSingle t_single = p_obj as FightSingle;
            //Debug.Log("t_single.name:" + t_single.name);

            if (!p_list.Contains(p_obj))
            {
                p_list.Add(p_obj);
            }
            //top
            if (t_single.Box_Index.m_index_row - 1 >= 0)
            {
                BoxIndex t_top = new BoxIndex(t_single.Box_Index.m_index_row - 1, t_single.Box_Index.m_index_col);
                FightSingle t_top_single = m_main.Fight_List[t_top.m_index_row, t_top.m_index_col] as FightSingle;
                if (t_top_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_top_single, t_single) && !t_top_single.HaveCheck)
                {
                    //Debug.Log("t_top_single.name:" + t_top_single.name);
                    t_top_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_top.m_index_row, t_top.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_top.m_index_row, t_top.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_top.m_index_row, t_top.m_index_col], p_list);
                }
            }
            //left
            if (t_single.Box_Index.m_index_col - 1 >= 0)
            {
                BoxIndex t_left = new BoxIndex(t_single.Box_Index.m_index_row, t_single.Box_Index.m_index_col - 1);
                FightSingle t_left_single = m_main.Fight_List[t_left.m_index_row, t_left.m_index_col] as FightSingle;
                if (t_left_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_left_single, t_single) && !t_left_single.HaveCheck)
                {
                    //Debug.Log("t_left_single.name:" + t_left_single.name);
                    t_left_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_left.m_index_row, t_left.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_left.m_index_row, t_left.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_left.m_index_row, t_left.m_index_col], p_list);
                }
            }
            //right
            if (t_single.Box_Index.m_index_col + 1 <= m_main.Fight_List.GetLength(1) - 1)
            {
                BoxIndex t_right = new BoxIndex(t_single.Box_Index.m_index_row, t_single.Box_Index.m_index_col + 1);
                FightSingle t_right_single = m_main.Fight_List[t_right.m_index_row, t_right.m_index_col] as FightSingle;
                if (t_right_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_right_single, t_single) && !t_right_single.HaveCheck)
                {
                    //Debug.Log("t_right_single.name:" + t_right_single.name);
                    t_right_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_right.m_index_row, t_right.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_right.m_index_row, t_right.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_right.m_index_row, t_right.m_index_col], p_list);
                }
            }
            //down
            if (t_single.Box_Index.m_index_row + 1 <= m_main.Fight_List.GetLength(0) - 1)
            {
                BoxIndex t_down = new BoxIndex(t_single.Box_Index.m_index_row + 1, t_single.Box_Index.m_index_col);
                FightSingle t_down_single = m_main.Fight_List[t_down.m_index_row, t_down.m_index_col] as FightSingle;
                if (t_down_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_down_single, t_single) && !t_down_single.HaveCheck)
                {
                    //Debug.Log("t_down_single.name:" + t_down_single.name);
                    t_down_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_down.m_index_row, t_down.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_down.m_index_row, t_down.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_down.m_index_row, t_down.m_index_col], p_list);
                }
            }
            //left_top
            if (t_single.Box_Index.m_index_row - 1 >= 0 && t_single.Box_Index.m_index_col - 1 >= 0)
            {
                BoxIndex t_lt = new BoxIndex(t_single.Box_Index.m_index_row - 1, t_single.Box_Index.m_index_col - 1);
                FightSingle t_lt_single = m_main.Fight_List[t_lt.m_index_row, t_lt.m_index_col] as FightSingle;
                if (t_lt_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_lt_single, t_single) && !t_lt_single.HaveCheck)
                {
                    //Debug.Log("t_lt_single.name:" + t_lt_single.name);
                    t_lt_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_lt.m_index_row, t_lt.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_lt.m_index_row, t_lt.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_lt.m_index_row, t_lt.m_index_col], p_list);
                }
            }
            //right_top
            if (t_single.Box_Index.m_index_row - 1 >= 0 && t_single.Box_Index.m_index_col + 1 <= m_main.Fight_List.GetLength(1) - 1)
            {
                BoxIndex t_rt = new BoxIndex(t_single.Box_Index.m_index_row - 1, t_single.Box_Index.m_index_col + 1);
                FightSingle t_rt_single = m_main.Fight_List[t_rt.m_index_row, t_rt.m_index_col] as FightSingle;
                if (t_rt_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_rt_single, t_single) && !t_rt_single.HaveCheck)
                {
                    //Debug.Log("t_rt_single.name:" + t_rt_single.name);
                    t_rt_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_rt.m_index_row, t_rt.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_rt.m_index_row, t_rt.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_rt.m_index_row, t_rt.m_index_col], p_list);
                }
            }
            //left_bottom
            if (t_single.Box_Index.m_index_row + 1 <= m_main.Fight_List.GetLength(0) - 1 && t_single.Box_Index.m_index_col - 1 >= 0)
            {
                BoxIndex t_lb = new BoxIndex(t_single.Box_Index.m_index_row + 1, t_single.Box_Index.m_index_col - 1);
                FightSingle t_lb_single = m_main.Fight_List[t_lb.m_index_row, t_lb.m_index_col] as FightSingle;
                if (t_lb_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_lb_single, t_single) && !t_lb_single.HaveCheck)
                {
                    //Debug.Log("t_lb_single.name:" + t_lb_single.name);
                    t_lb_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_lb.m_index_row, t_lb.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_lb.m_index_row, t_lb.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_lb.m_index_row, t_lb.m_index_col], p_list);
                }
            }
            //right_bottom
            if (t_single.Box_Index.m_index_row + 1 <= m_main.Fight_List.GetLength(0) - 1 && t_single.Box_Index.m_index_col + 1 <= m_main.Fight_List.GetLength(1) - 1)
            {
                BoxIndex t_rb = new BoxIndex(t_single.Box_Index.m_index_row + 1, t_single.Box_Index.m_index_col + 1);
                FightSingle t_rb_single = m_main.Fight_List[t_rb.m_index_row, t_rb.m_index_col] as FightSingle;
                if (t_rb_single.GetIcon.icon == t_single.GetIcon.icon && m_main.IsFriend(t_rb_single, t_single) && !t_rb_single.HaveCheck)
                {
                    //Debug.Log("t_rb_single.name:" + t_rb_single.name);
                    t_rb_single.HaveCheck = true;
                    if (!p_list.Contains(m_main.Fight_List[t_rb.m_index_row, t_rb.m_index_col]))
                    {
                        p_list.Add(m_main.Fight_List[t_rb.m_index_row, t_rb.m_index_col]);
                    }
                    GetAbleList(m_main.Fight_List[t_rb.m_index_row, t_rb.m_index_col], p_list);
                }
            }
        }

        //筛选掉不符合条件的item************************
        void MatchItem(List<GObject> p_list)
        {
            for (int i = 0; i < p_list.Count; i++)
            {
                FightSingle t_single = p_list[i] as FightSingle;
                for (int j = 0; j < p_list.Count; j++)
                {
                    FightSingle t_match_single = p_list[j] as FightSingle;
                    if (t_match_single != t_single && m_main.IsFriend(t_match_single, t_single))
                    {
                        t_single.MatchCount++;
                        t_match_single.MatchCount++;
                    }
                }
            }

            int t_match_2_count = 0;
            for (int i = 0; i < p_list.Count; i++)
            {
                FightSingle t_single = p_list[i] as FightSingle;
                if (t_single.MatchCount == 2)
                {
                    t_match_2_count++;
                    if (t_match_2_count > 2)
                    {
                        p_list.Remove(p_list[i]);
                        i--;
                    }
                }
            }
        }

        public override FightAction TurnNextAction()
        {
            //Debug.Log("m_main.Comb_List.Count:" + m_main.Comb_List.Count);
            return m_main.Comb_List.Count > 0 ? FightAction.Line : FightAction.Rebuild;
        }
    }
}


