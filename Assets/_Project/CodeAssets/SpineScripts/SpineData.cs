using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class SpineData : MonoBehaviour
{
    private SkeletonAnimation m_anim;
    private AIAction m_action;

	private PetBaseTemplate m_template;
    
    private Dictionary<string, Spine.Animation> m_spine_anim_dic = new Dictionary<string, Spine.Animation>();

	private Dictionary<AIAction,string[]> m_action_dic = new Dictionary<AIAction, string[]> ()
	{ 
		{AIAction.Idle,new string[]{ "tand" } },
		{AIAction.Walk,new string[]{ "walk" } },
		{AIAction.Sleep,new string[]{ "sleep_1","sleep_2","sleep_3" } },
		{AIAction.Eat,new string[]{ "hungry_3" } },
		{AIAction.Shit,new string[]{ "toilet" } },
		{AIAction.Beg_Food,new string[]{ "hungry_1","hungry_2", "hungry_3" } },
		{AIAction.Beg_Touch,new string[]{ "seektouch_1","seektouch_2", "seektouch_3" } },
		{AIAction.Beg_Shower,new string[]{ "tackeabath_1", "tackeabath_2", "tackeabath_3" } },
		{AIAction.Be_Caught,new string[]{ "catch_1","catch_2" } },
        {AIAction.Be_Caught_Dis,new string[]{ "catch_3" } },
        {AIAction.Be_Touched,new string[]{ "fodle_1","fodle_2","fodle_3" } },
		{AIAction.Be_Bathed,new string[]{ "bebathed_1","bebathed_2","bebathed_3" } },
		{AIAction.Beat_Other_Animals,new string[]{ "tand","tand_free_1","tand_free_2","tand_free_3" } },
		{AIAction.Be_Beat_By_Other_Animals,new string[]{ "tand","tand_free_1","tand_free_2","tand_free_3" } },
		{AIAction.Medicine,new string[]{ "medicine_1","medicine_2","medicine_3" } },
		{AIAction.Eat_Wrong_Medicine,new string[]{ "tand","tand_free_1","tand_free_2","tand_free_3" } },
        {AIAction.Idle_Free,new string[]{ "tand_free_1","tand_free_2","tand_free_3" } },
    };

    void Awake()
    {
        m_anim = GetComponent<SkeletonAnimation>();
    }

    public SkeletonAnimation SkeletonAnim()
    {
        return m_anim;
    }

    //设置宠物
    public void SetId(int p_id)
    {
        foreach (Spine.Animation t_anim in m_anim.Skeleton.Data.Animations)
		{
			//Debug.Log(t_anim.name + ":" + t_anim.Duration + "+" + t_anim.Timelines.Count);
			if(!m_spine_anim_dic.ContainsKey(t_anim.Name))
			{
				m_spine_anim_dic.Add(t_anim.name,t_anim);
			}
		}

		m_template = PetBaseTemplate.GetPetBaseTemplateById(p_id);
    }

    //获得动画播放时间
    public float GetDurationTime (AIAction p_action,bool p_last_duration = false)
	{
		float t_time = 0;
		string[] t_actions = m_action_dic [p_action];
        if (p_last_duration)
        {
            t_time = m_spine_anim_dic[m_template.ActLead + t_actions[t_actions.Length - 1]].Duration;
        }
        else
        {
            for (int i = 0; i < t_actions.Length; i++)
            {
                t_time += m_spine_anim_dic[m_template.ActLead + t_actions[i]].Duration;
            }
        }

        //Debug.Log ("t_time:" + t_time);
        return t_time;
	}

    //动画播放
	public void AnimalAction(AIAction p_action, bool p_end = false)
    {
		string[] t_actions = m_action_dic [p_action];

        m_anim.skeleton.SetToSetupPose();
        m_anim.state.ClearTracks();
        if (t_actions.Length > 1) 
		{
			if(p_action == AIAction.Be_Caught)
            {
                if (p_end)
                {
                    m_anim.state.SetAnimation(0, m_template.ActLead + t_actions[t_actions.Length - 1], false);
                }
                else
                {
                    m_anim.state.SetAnimation(0, m_template.ActLead + t_actions[0], false);
                    m_anim.state.AddAnimation(0, m_template.ActLead + t_actions[1], true, 0);
                }
            }
			else 
			{
				m_anim.state.SetAnimation (0, m_template.ActLead + t_actions [0], false);
				for (int i = 1; i < t_actions.Length; i++)
                {
					m_anim.state.AddAnimation (0, m_template.ActLead + t_actions [i], false, 0);
				}
			}
		} 
		else 
		{
			m_anim.state.SetAnimation (0, m_template.ActLead + t_actions [0], p_action == AIAction.Walk || p_action == AIAction.Idle);
		}
    }
}
