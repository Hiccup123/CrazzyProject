using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public enum StateID
{
    State_Null = 0,
    State_Idle = 1,
    State_Move = 2,
    State_Sleep = 3,
    State_Eat = 4,
    State_Shit = 5,
    State_Beg_Food = 6,
    State_Beg_Touch = 7,
    State_Beg_Shower = 8,
    State_Be_Caught = 9,
    State_Be_Touch = 10,
    State_Be_Shower = 11,
    State_Eat_Wrong_Medchine = 12,
    State_Eat_Medchine = 13,
}

public enum TCondition
{
    C_Null = 0,
    C_Idle = 1,
    C_Move = 2,
    C_Sleep = 3,
    C_Eat = 4,
    C_Shit = 5,
    C_Beg_Food = 6,
    C_Beg_Touch = 7,
    C_Beg_Shower = 8,
    C_Be_Caught = 9,
    C_Be_Touch = 10,
    C_Be_Shower = 11,
    C_Eat_Wrong_Medchine = 12,
    C_Eat_Medchine = 13,
}

public abstract class CAIState
{
    protected Dictionary<TCondition, StateID> m_state_dic = new Dictionary<TCondition, StateID>();

    protected StateID m_state_id;
    public StateID State_ID { get { return m_state_id; } }

    //增加关联树
    public void AddTCondition(TCondition p_condition, StateID p_id)
    {
        if (p_condition == TCondition.C_Null || p_id == StateID.State_Null)
        {
            Debug.LogError("TCondition:" + p_condition + " or " + "StateID:" + p_id + " is null");
            return;
        }
        if (m_state_dic.ContainsKey(p_condition))
        {
            Debug.Log("Aleady containsKey" + p_condition);
            return;
        }
        Debug.Log("AddCondition:" + p_condition + "+" + p_id + "+" + m_state_dic.Count);
        m_state_dic.Add(p_condition, p_id);
    }

    //删除一个关联树
    public void DeleteTCondition(TCondition p_condition)
    {
        if (p_condition == TCondition.C_Null)
        {
            Debug.LogError("Error:null condition is not allowed");
        }

        if (m_state_dic.ContainsKey(p_condition))
        {
            m_state_dic.Remove(p_condition);
            return;
        }
        Debug.LogError("Error:TCondition:" + p_condition.ToString() + "was not on the state's condition list");
    }

    //检索当前状态
    public StateID GetOutPutState(TCondition p_condition)
    {
        Debug.Log("GetOutPutState:" + m_state_dic.ContainsKey(p_condition));
        if (m_state_dic.ContainsKey(p_condition))
        {
            return m_state_dic[p_condition];
        }
        Debug.Log("Not contains");
        return StateID.State_Null;
    }

    public virtual void DoBeforeEnter()
    {

    }

    public virtual void DoBeforeLeave()
    {

    }

    public abstract void ByCondition(); //监听环境条件的改变并触发相应的事件转换
    public abstract void Action();      //表现当前状态下npc行为
}
