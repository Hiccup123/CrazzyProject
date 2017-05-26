using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

public class CMove : CAIState {

    private CAIControl m_control;

    private float m_speed = 50;                 //移动速度
    private Vector3 m_cur_pos;                  //当前位置
    private Vector3 m_end_pos = Vector3.zero;   //目标位置
    private Vector3 m_start_pos = Vector3.zero; //起始走动位置
    private float m_total_time = 0.0f;          //总用时
    private float m_cost_time = 0.0f;           //已经花费的时间
    private float m_time_precent = 0.0f;        //时间比率
    private bool m_dir_left = true;             //宠物朝向

    public CMove(CAIControl p_control)
    {
        m_control = p_control;
        m_state_id = StateID.State_Move;
    }

    public override void ByCondition()
    {
        Debug.Log("Move_Condition");
        if (m_time_precent >= 1)
        {
            m_control.SetTCondition(TCondition.C_Idle);
        }
    }

    public override void Action()
    {
        Debug.Log("Move_Action");
        MoveByTimeLine();
    }

    private void MoveByTimeLine()
    {
        /*
             * 获得移动的最终目标位置，根据移动速度获得一共需要移动的时间 totalTime
             * 每一帧，
             *   1、累加 已经逝去的时间，并得到costTime，并获得移动的百分比 precent = costTime/totalTime
             *   2、获得当前精灵的位置，根据precent 进行位置插值，得到这一帧应该移动的位置
             *   3、使用设置移动
             *   4、通过precent判断是否<1 来判断是否移动到了目标位置
             *   5、如果完成，则调用最后一次移动实现，终点移动误差，并置为一些标志位
             */
        //获得当前位置
        m_cur_pos = m_control.position;//p_cur_pos

        //如果时间百分比小于1 说明还没有移动到终点
        if (m_time_precent < 1)
        {
            //累加时间
            m_cost_time += Time.deltaTime;
            m_time_precent = m_cost_time / m_total_time;

            Vector3 t_target = Vector3.Lerp(m_start_pos, m_end_pos, m_time_precent);

            m_control.position = t_target;
        }
        else //大于或者等于1 了说明是最后一次移动
        {
            m_control.position = m_end_pos;
            m_end_pos = Vector3.zero;
            m_time_precent = 0.0f;
            m_cost_time = 0.0f;
            m_control.SetAnimalAction(false);
        }
    }

    public override void DoBeforeLeave()//离开这个状态时
    {
        
    }

    public override void DoBeforeEnter()//进入这个状态时
    {
        m_control.AnimalAction(0, -1, 0, -1);
        m_control.SetAnimalAction(true);
        m_start_pos = m_cur_pos;

        //获得移动终点位置
        m_end_pos = m_control.RandomPos();
        m_end_pos.z = 0;

        m_cost_time = 0.0f;
        //计算记录
        var t_sub_vector3 = m_end_pos - m_cur_pos;

        m_dir_left = t_sub_vector3.x <= 0;
        m_control.SetAnimalDir(m_dir_left);

        //计算需要移动的总时间
        m_total_time = t_sub_vector3.magnitude / m_speed;
    }
}
