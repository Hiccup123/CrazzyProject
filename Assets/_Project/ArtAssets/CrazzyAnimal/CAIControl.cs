using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class CAIControl : GButton {

    private CAIManager m_c_ai_manager;

    public BaseData m_data;

    private GMovieClip m_animal_icon;
    private Rect m_drag_rect;

    public override void ConstructFromXML(XML cxml)
    {
        base.ConstructFromXML(cxml);

        m_animal_icon = GetChild("n16").asMovieClip;
        Debug.Log("ConstructFromXML");
        //InItState();
    }

    void InItState()
    {
        CIdle p_idle = new CIdle(this);
        p_idle.AddTCondition(TCondition.C_Idle, StateID.State_Idle);
        CMove p_move = new CMove(this);
        p_move.AddTCondition(TCondition.C_Move, StateID.State_Move);
        CSleep p_sleep = new CSleep(this);
        p_sleep.AddTCondition(TCondition.C_Sleep, StateID.State_Sleep);


        m_c_ai_manager = new CAIManager();
        m_c_ai_manager.AddState(p_idle);
        m_c_ai_manager.AddState(p_move);
        m_c_ai_manager.AddState(p_sleep);
        Debug.Log("InItState");
    }

    public void InItItem (Rect p_rect, BaseData p_data)
    {
        m_drag_rect = p_rect;
        m_data = p_data;
        this.dragBounds = p_rect;
        //this.onClick.Add(ClickAnimal);
        m_animal_icon.playing = false;
        //m_hand.visible = false;
        InItState();
    }

    //转换状态
    public void SetTCondition(TCondition p_condition)
    {
        m_c_ai_manager.PerformTCondition(p_condition);
    }

    protected override void OnUpdate()
    {
        Debug.Log("OnUpdate");
        m_c_ai_manager.CurCAIState.ByCondition();
        m_c_ai_manager.CurCAIState.Action();
    }

    //随机动作
    public int RandomValue ()
    {
        return Random.Range(0,3);
    }

    //随机位置
    public Vector3 RandomPos ()
    {
        float t_x = Random.Range(-100, 100);
        float t_y = Random.Range(-100, 100);

        if (x + t_x < 0)
        {
            t_x = 35 - x;
        }
        if (x + t_x > 500)
        {
            t_x = 465 - x;
        }
        if (y + t_y < 0)
        {
            t_y = -y;
        }
        if (y + t_y > 530)
        {
            t_y = 530 - y;
        }

        return new Vector3(x + t_x,y + t_y,0);
    }

    public void SetAnimalAction(bool p_value)
    {
        m_animal_icon.playing = p_value;
    }

    public void SetAnimalDir (bool p_value)
    {
        m_animal_icon.scaleX = p_value ? -1 : 1;
    }

    public void AnimalAction(int p_start,int p_end,int p_loop_time,int p_end_fram,EventCallback0 p_call_back0 = null)
    {
        m_animal_icon.SetPlaySettings(p_start,p_end,p_loop_time,p_end_fram);
        m_animal_icon.onPlayEnd.Add(() => { if (p_call_back0 != null) { p_call_back0(); } });
    }
}
