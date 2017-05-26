using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

namespace Fight
{
    public class FightLine : FightBase
    {
        private bool m_touched;
        private bool m_mouse_up;

        public FightLine(FightMain p_data) : base(p_data)
        {

        }

        public override void OnExcute()
        {
            m_touched = false;
            m_mouse_up = false;
        }

        public override void InputUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //获得当前触摸的对象
                m_main.Cur_Obj = GRoot.inst.touchTarget.parent == null ? null : GRoot.inst.touchTarget.parent;
                if (m_main.Cur_Obj != null && m_main.Cur_Obj.name.Length > 5 && m_main.Cur_Obj.name.Substring(0, 5) == "FItem")
                {
                    m_touched = true;
                    m_main.Temp_Obj = m_main.Cur_Obj;
                    m_main.Dispose_List.Add(m_main.Cur_Obj);
                    //Debug.Log("m_find_count:" + m_main.Dispose_List.Count);

                    FightSingle t_single = m_main.Cur_Obj as FightSingle;
                    //t_single.GetIcon.color = new Color32(255, 102, 102, 255);
                    Tweener t_tweener = t_single.GetIcon.TweenScale(Vector3.one * 1.5f, 0.2f);
                    t_tweener.OnComplete(() =>
                    {
                        t_single.GetIcon.TweenScale(Vector3.one, 0.1f);
                    });

                    t_single.SoundPlay(401);
                    m_main.GetAlphaList(t_single.Type);
                }
            }

            if (m_touched)
            {
                if (FightMain.S_Hover_Obj != m_main.Temp_Obj)
                {
                    //相同类型item并相邻则加入可消除集合中
                    if (Check())
                    {
                        m_main.Temp_Obj = FightMain.S_Hover_Obj;
                        m_main.Dispose_List.Add(FightMain.S_Hover_Obj);

                        FightSingle t_single = m_main.Temp_Obj as FightSingle;
                        t_single.SoundPlay(m_main.Dispose_List.Count == 5 ? 404 : 401);
                        //t_single.GetIcon.color = new Color32(255, 102, 102, 255);
                        Tweener t_tweener = t_single.GetIcon.TweenScale(Vector3.one * 1.5f, 0.2f);
                        t_tweener.OnComplete(() =>
                        {
                            t_single.GetIcon.TweenScale(Vector3.one, 0.1f);
                        });

                        m_main.AddArrow();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    m_touched = false;
                    m_main.SetAlphList();
                    m_mouse_up = true;
                }
            }
        }

        public override FightStatus OnUpdate()
        {
            if (m_mouse_up)
            {
                return FightStatus.Finish;
            }

            return FightStatus.Running;
        }

        //检测判定当前item是否可消除
        private bool Check()
        {
            GObject t_hover_obj = FightMain.S_Hover_Obj;
            FightSingle t_hover_single = t_hover_obj as FightSingle;
            FightSingle t_temp_single = m_main.Temp_Obj as FightSingle;

            if (t_hover_single == null || t_temp_single == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(t_hover_single.GetIcon.icon) || string.IsNullOrEmpty(t_temp_single.GetIcon.icon))
            {
                return false;
            }
            if (!m_main.IsFriend(t_hover_single, t_temp_single))
            {
                //Debug.Log("hover:" + t_hover_single.GetIcon.icon + "   And   " + "temp:" + t_temp_single.GetIcon.icon);
                //Debug.Log("hover:" + t_hover_single.name + "   And   " + "temp:" + t_temp_single.name);
                return false;
            }

            for (int i = 0; i < m_main.Dispose_List.Count; i++)
            {
                FightSingle t_single = m_main.Dispose_List[i] as FightSingle;
                //如果将要被消除的集合中包含手指当前停留的item
                if (t_single.name.Equals(t_hover_single.name))
                {
                    //如果当前停留的item是倒数第二个
                    if (i >= 0 && i == m_main.Dispose_List.Count - 2)
                    {
                        //取消最后一个item的选中状态
                        FightSingle h_single = m_main.Dispose_List[m_main.Dispose_List.Count - 1] as FightSingle;
                        //h_single.GetIcon.color = Color.white;
                        h_single.GetIcon.TweenScale(Vector3.one, 0.1f);

                        //var t_image = m_recycle[m_recycle.Count - 1];
                        //m_recycle.Remove(t_image);
                        //t_image.Dispose();
                        m_main.RemoveArrow();

                        m_main.Dispose_List.Remove(m_main.Dispose_List[m_main.Dispose_List.Count - 1]);
                        m_main.Temp_Obj = FightMain.S_Hover_Obj;
                    }
                    return false;
                }
                else
                {

                }
            }

            for (int i = 0; i < m_main.Dispose_List.Count; i++)
            {
                FightSingle t_single = m_main.Dispose_List[i] as FightSingle;
                if (!t_hover_single.GetIcon.icon.Equals(t_single.GetIcon.icon))
                {
                    return false;
                }
            }

            m_main.Cur_Obj = t_hover_obj;

            return true;
        }

        public override FightAction TurnNextAction()
        {
            if (m_main.Dispose_List.Count >= m_main.Min_Dis_Count)
            {
                return FightAction.Remove;
            }
            else
            {
                //Debug.Log("Cancel");
                return FightAction.Cancel_Line;
            }
        }
    }
}

