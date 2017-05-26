using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightAction
{
    Free = 0,       //闲置
    Line = 1,       //连线
    Cancel_Line = 2,    //取消连线
    Remove = 3,     //消除
    Repair = 4,     //修补
    Check_Comb = 5,      //检测组合
    Rebuild = 6,    //重建
}

public enum FightStatus
{
    Running = 0,
    Finish = 1,
}

//item下标
public struct BoxIndex
{
    public int m_index_row;
    public int m_index_col;

    public BoxIndex(int p_x, int p_y)
    {
        m_index_row = p_x;
        m_index_col = p_y;
    }
}

public enum PlayerType
{
    P_Self = 0,     //自己
    P_Enemy = 1,    //敌人
}
