using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class BaseData {

    public BaseData ()
    {

    }

    public int m_id;        //id
    public int m_level;     //等级
    public int m_hp;        //生命值
    public int m_force;     //武力值
    public int m_charm;     //魅力值
    public int m_health;    //健康值
    public int m_hunger;    //饥饿值
    public int m_clean;     //清洁值
    public int m_appease;   //安抚值
    public int m_grow;      //成长值
}

public class AIContext
{
    public AIContext ()
    {

    }

    public GObject m_root;
    public Vector3 m_root_pos;
    public GObject m_target;
    public Vector3 m_target_pos;
}

public enum ControlState
{
    Zhua = 0,       //抓
    Food = 1,       //喂食
    Shower = 2,     //洗澡
    ChanShi = 3,    //铲屎
    FuMo = 4,       //抚摸
    ZhiLiao = 5,    //治疗
    KongXian = 6,   //空闲状态
}
