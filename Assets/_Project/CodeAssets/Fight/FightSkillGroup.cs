using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

namespace Fight
{
    public class FightSkillGroup : GComponent
    {
        private GGraph m_show_graph;
        private GGraph m_skill_graph;

        private SpineAnim m_show_anim;
        private SpineAnim m_skill_anim;

        private PlayerType m_player_type;
        private EventDelegate m_stop_event;
        private EventDelegate m_end_event;

        private GObjectPool m_wrap_pool;

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_show_graph = GetChild("Skill1").asGraph;
            m_skill_graph = GetChild("Skill2").asGraph;

            m_show_anim = GameManager.LoadObj(m_show_graph, new Vector3(0, 367), "Effect/BearSkill", new Vector3(325, -120), Vector3.one).GetComponent<SpineAnim>();
            m_skill_anim = GameManager.LoadObj(m_skill_graph, Vector3.zero, "Effect/Effect", Vector3.zero, Vector3.one * 100).GetComponent<SpineAnim>();

            m_wrap_pool = new FairyGUI.GObjectPool(this.displayObject.cachedTransform);
        }

        //播放展示动画
        public void SkillShow(PlayerType p_type,EventDelegate p_stop_event,EventDelegate p_end_event)
        {
            m_player_type = p_type;
            m_stop_event = p_stop_event;
            m_end_event = p_end_event;

            m_show_anim.SetEvent("end", ShowEnd);
            m_show_anim.PlayAnim("bear_skill",false);
        }

        //播放技能动画
        void ShowEnd ()
        {
            m_skill_graph.position = new Vector3(325,m_player_type == PlayerType.P_Self ? 960 : 0);
            m_skill_anim.gameObject.transform.localRotation = new Quaternion(0,0,m_player_type == PlayerType.P_Self ? 0 : 180,0);

            m_skill_anim.SetEvent("end", SkillEnd);
            m_skill_anim.SetEvent("stop", Stop);
            m_skill_anim.PlayAnim("pet_arrow", false);
        }

        void Stop()
        {
            if(m_stop_event != null)
            {
                m_stop_event();
            }
        }

        //技能动画播放完毕
        void SkillEnd ()
        {
            if(m_end_event != null)
            {
                m_end_event();
            }
        }

        //创建特效
        public void GetEffectFromPool (PlayerType p_type,Vector3 p_pos,EventDelegate p_end_event,bool m_five)
        {
            GObject t_effect = m_wrap_pool.GetObject(UIPackage.GetItemURL("EffectPack", "FAttackSingle"));
            t_effect.position = p_pos + new Vector3(58, 180);
            this.AddChild(t_effect);
            FightAttackSingle t_single = t_effect as FightAttackSingle;
            t_single.InItEffect(new Vector3(Random.Range(450,600), p_type == PlayerType.P_Self ? Random.Range(120,150) : Random.Range(785, 900)), p_type == PlayerType.P_Self, m_five, p_end_event,EffectEndEvent);
        }

        void EffectEndEvent (GObject p_obj)
        {
            m_wrap_pool.ReturnObject(p_obj);
        }
    }
}

