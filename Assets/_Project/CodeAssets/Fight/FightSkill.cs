using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using Spine.Unity;

public class FightSkill : MonoBehaviour {

    private SkeletonAnimation m_skill_anim;
    private Dictionary<string,Spine.Animation> m_skill_dic = new Dictionary<string, Spine.Animation>(); 

    void Awake ()
    {
        m_skill_anim = GetComponent<SkeletonAnimation>();
        foreach (Spine.Animation t_anim in m_skill_anim.Skeleton.Data.Animations)
        {
            //Debug.Log(t_anim.name + ":" + t_anim.Duration + "+" + t_anim.Timelines.Count);
            m_skill_dic.Add(t_anim.name,t_anim);
        }
    }

    //被攻击提示
    public void BeAttackPlay ()
    {
        m_skill_anim.state.SetAnimation(0, m_skill_dic["hurt_light"], false);
    }

    //移除itemEffect
    public void RemoveEffect (bool p_five)
    {
        m_skill_anim.skeleton.SetToSetupPose();
        m_skill_anim.state.ClearTracks();
        
        if(p_five)
        {
            m_skill_anim.state.SetAnimation(0, m_skill_dic["pet_gatherlight"], false);
            m_skill_anim.state.AddAnimation(0, m_skill_dic["pet_effect_disappear"], false,0);
        }
        else
        {
            m_skill_anim.state.SetAnimation(0, m_skill_dic["pet_effect_disappear"], false);
        }
    }

    public float GetDurationTime (string p_name)
    {
        return m_skill_dic[p_name].Duration;
    }
}
