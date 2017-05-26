using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

public abstract class AIBase {
    
    public AIAction m_action;   //当前状态

    public float m_duration;    //状态完成需要的时间
    public float m_cur_duration;    //完成状态当前消耗时间

    public SpineData m_data;

    public bool m_init = false;

    public AIBase(SpineData p_data)
    {
        m_data = p_data;
    }

    public virtual void EnterState(AIContext p_context)      //进入状态
    {
        m_cur_duration = 0;
        m_data.AnimalAction(m_action);
        m_init = true;
    }

    public virtual AIStatus OnUpdate()  //游戏主循环中状态的内部执行机制
    {
        if (!m_init)
        {
            return AIStatus.Fail;
        }
        m_cur_duration += Time.deltaTime * 0.5f;
        //Debug.Log("m_cur_duration:" + m_cur_duration);
        if (m_cur_duration >= m_duration)
        {
            return AIStatus.Success;
        }

        return AIStatus.Running;
    }

    public virtual void ExitState ()        //切换状态，即退出当前状态时
    {
        m_init = false;
        m_data.AnimalAction(AIAction.Idle);
    }     

    public virtual void OtherEvent ()    //其它可能事件
    {

    }
}

#region Idle Free
public class AIIdleFree : AIBase
{
    public AIIdleFree (SpineData p_data) : base (p_data)
    {
        m_action = AIAction.Idle_Free;
        m_duration = p_data.GetDurationTime(m_action);
        //Debug.Log("m_duration:" + m_duration);
    }
}
#endregion

#region Walk
public class AIWalk : AIBase
{
    private AIContext m_context;

    private float m_speed = 50;                 //移动速度
    private Vector3 m_start_pos;                //起始位置
    private Vector3 m_end_pos = Vector3.zero;   //目标位置
    private Vector3 m_cur_pos;                  //当前位置
    private float m_total_time = 0.0f;          //总用时
    private float m_cost_time = 0.0f;           //已经花费的时间
    private float m_time_precent = 0.0f;        //时间比率

    public AIWalk (SpineData p_data) : base (p_data)
    {
        m_action = AIAction.Walk;
    }

    public override void EnterState(AIContext p_context)
    {
        base.EnterState(p_context);
        m_context = p_context;

        m_start_pos = p_context.m_root_pos;
        m_end_pos = p_context.m_target_pos;

        m_total_time = 0;
        m_cost_time = 0;
        m_time_precent = 0;

        //计算记录
        var t_sub_vector3 = m_end_pos - m_start_pos;

        m_data.SkeletonAnim().skeleton.FlipX = t_sub_vector3.x > 0;

        //m_top.SetHandPos(t_sub_vector3.x < 0);

        //计算需要移动的总时间
        m_total_time = t_sub_vector3.magnitude / m_speed;
    }

    public override AIStatus OnUpdate()
    {
        if (!m_init)
        {
            return AIStatus.Fail;
        }

        //如果时间百分比小于1 说明还没有移动到终点
        if (m_time_precent < 1)
        {
            //累加时间
            m_cost_time += Time.deltaTime;
            m_time_precent = m_cost_time / m_total_time;

            m_cur_pos = Vector3.Lerp(m_start_pos, m_end_pos, m_time_precent);

            m_context.m_root.position = m_cur_pos;

            return AIStatus.Running;
        }

        return AIStatus.Success;
    }
}
#endregion

#region Sleep
public class AISleep : AIBase
{
    public AISleep (SpineData p_data) : base (p_data)
    {
        m_action = AIAction.Sleep;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region Shit
public class AIShit : AIBase
{
    public AIShit(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Shit;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region EatFood
public class AIEat : AIBase
{
    public AIEat(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Eat;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region BegFood
public class AIBegFood : AIBase
{
    public AIBegFood (SpineData p_data) : base (p_data)
    {
        m_action = AIAction.Beg_Food;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region BegTouch
public class AIBegTouch : AIBase
{
    public AIBegTouch(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Beg_Touch;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region BegShower
public class AIBegShower : AIBase
{
    public AIBegShower(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Beg_Shower;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region BeCaught
public class AIBeCaught : AIBase
{
    public bool M_Touching;

    public AIBeCaught(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Be_Caught;
    }

    public override void EnterState(AIContext p_context)
    {
        M_Touching = true;
        base.EnterState(p_context);
    }

    public override void OtherEvent()
    {
        M_Touching = false;
    }

    public override AIStatus OnUpdate()
    {
        if (!m_init)
        {
            return AIStatus.Fail;
        }

        if (M_Touching)
        {
            return AIStatus.Running;
            
        }

        return AIStatus.Fail;
    }
}
#endregion

#region BeCaughtDis
public class AIBeCaughtDis : AIBase
{
    public AIBeCaughtDis(SpineData p_data) : base (p_data)
    {
        m_action = AIAction.Be_Caught_Dis;
        m_duration = p_data.GetDurationTime(m_action, true);
    }
}
#endregion

#region BeTouched
public class AIBeTouched : AIBase
{
    public AIBeTouched (SpineData p_data) : base (p_data)
    {
        m_action = AIAction.Be_Touched;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region BeBathed
public class AIBeBathed : AIBase
{
    public AIBeBathed(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Be_Bathed;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region BeatOtherAnimals
public class AIBeatOtherAnimals : AIBase
{
    public AIBeatOtherAnimals (SpineData p_data) : base (p_data)
    {
        m_action = AIAction.Beat_Other_Animals;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region BeBeatByOtherAnimals
public class AIBeBeatByOtherAnimals : AIBase
{
    public AIBeBeatByOtherAnimals(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Be_Beat_By_Other_Animals;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region Medicine
public class AIMedicine : AIBase
{
    public AIMedicine(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Medicine;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion

#region WrongMedicine
public class AIWrongMedicine : AIBase
{
    public AIWrongMedicine(SpineData p_data) : base(p_data)
    {
        m_action = AIAction.Eat_Wrong_Medicine;
        m_duration = p_data.GetDurationTime(m_action);
    }
}
#endregion