using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class CAIManager
{
    private List<CAIState> m_states;

    private StateID m_cur_state_id;     //记录当前stateID
    public StateID CurStateID { get { return m_cur_state_id; } }

    private CAIState m_cur_c_ai_state;  //记录当前状态
    public CAIState CurCAIState { get { return m_cur_c_ai_state; } }

    public CAIManager()
    {
        m_states = new List<CAIState>();
    }

    //增加状态转换树
    public void AddState(CAIState p_state)
    {
        if(p_state == null)
        {
            Debug.LogError("Error:null state is not not allowed");
            return;
        }
        if(m_states.Count == 0)
        {
            m_states.Add(p_state);
            m_cur_c_ai_state = p_state;
            m_cur_state_id = p_state.State_ID;
            return;
        }
        foreach(CAIState t_state in m_states)
        {
            if(t_state.State_ID == p_state.State_ID)
            {
                Debug.LogError("Error:" + p_state + "has already been added");
                return;
            }
        }
        Debug.Log("AddState:" + p_state);
        m_states.Add(p_state);
    }

    //移除状态转换树
    public void DeleteState(StateID p_id)
    {
        if(p_id == StateID.State_Null)
        {
            Debug.LogError("Error:StateID is not allowed for a real state");
            return;
        }

        foreach(CAIState t_state in m_states)
        {
            if(t_state.State_ID == p_id)
            {
                m_states.Remove(t_state);
                return;
            }
        }
        Debug.Log("Error:not on the list of m_states");
    }

    //执行转换
    public void PerformTCondition(TCondition p_condition)
    {
        if(p_condition == TCondition.C_Null)
        {
            Debug.LogError("Error:null condition is not allowed for a real condition");
            return;
        }

        StateID t_id = m_cur_c_ai_state.GetOutPutState(p_condition);
        if(t_id == StateID.State_Null)
        {
            Debug.LogError("Error:state does not have a target state");
            return;
        }

        m_cur_state_id = t_id;

        foreach(CAIState t_state in m_states)
        {
            if(t_state.State_ID == m_cur_state_id)
            {
                m_cur_c_ai_state.DoBeforeLeave();//转换之前do something
                m_cur_c_ai_state = t_state;
                m_cur_c_ai_state.DoBeforeEnter();//状态转换完后，为新状态do something
                break;
            }
        }
    }
}
