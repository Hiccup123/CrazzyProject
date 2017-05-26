using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStatus
{
    Running = 0,
    Success = 1,
    Fail = 2,
}

public enum AIAction {

	Idle = 0,                       //待机
    Walk = 1,                       //走动
    Sleep = 2,                      //睡觉
    Eat = 3,                        //吃食
    Shit = 4,                       //大便
    Beg_Food = 5,                   //求喂食
    Beg_Touch = 6,                  //求抚摸
    Beg_Shower = 7,                 //求洗澡
    Be_Caught = 8,                  //被抓起
    Be_Touched = 9,                 //被抚摸
    Be_Bathed = 10,                 //被洗澡
    Beat_Other_Animals = 11,        //追打其它动物
    Be_Beat_By_Other_Animals = 12,  //被其它动物追打
    Medicine = 13,                  //吃药
    Eat_Wrong_Medicine = 14,        //吃错药
    Idle_Free = 15,                 //站立随机
    Be_Caught_Dis = 16,             //被抓起放下后
}
