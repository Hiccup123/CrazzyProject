using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class CIdle : CAIState {

    private CAIControl m_control;

    private float m_think_time = 1; //思考时间
    private float m_last_think_time;    //上次思考时间

    public CIdle(CAIControl p_control)
    {
        m_control = p_control;
        m_state_id = StateID.State_Idle;
    }

    public override void ByCondition()
    {
        Debug.Log("Idle_Condition");
        if (Time.time - m_last_think_time > m_think_time)
        {
            m_last_think_time = Time.time;
            m_think_time = Random.Range(4, 7);
            m_control.SetTCondition(TCondition.C_Move);
        }
    }

    public override void Action()
    {
        Debug.Log("Idle_Action");
    }

    public override void DoBeforeEnter()
    {
        
    }

    public override void DoBeforeLeave()
    {
        
    }
}
