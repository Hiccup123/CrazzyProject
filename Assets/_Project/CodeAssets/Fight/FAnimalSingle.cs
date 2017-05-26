using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

namespace Fight
{
    public class FAnimalSingle : GButton
    {
        private FAnimalData m_data;
        private FightEvent m_attack;

        private GLoader m_bg;
        private GLoader m_icon;
        private GLoader m_type_icon;
        private GProgressBar m_power_bar;
        private GLoader m_power_icon;
        private GLoader m_bar_bg;
        private FightSingle m_single;
        private GGroup m_group;
        private GGraph m_effect_graph;

        private PlayerType m_type;
        private EventDelegate m_click_event;

        private SpineAnim m_anim;

        public override void ConstructFromXML(XML cxml)
        {
            base.ConstructFromXML(cxml);

            m_bg = GetChild("Bg").asLoader;
            m_icon = GetChild("HeadIcon").asLoader;
            m_type_icon = GetChild("TypeIcon").asLoader;
            m_power_bar = GetChild("PowerBar").asProgress;
            m_bar_bg = m_power_bar.GetChild("Bg").asLoader;
            m_power_icon = m_power_bar.GetChild("PowerIcon").asLoader;
            m_single = GetChild("Null").asCom as FightSingle;
            m_group = GetChild("AnimalObj").asGroup;

            m_effect_graph = GetChild("Effect").asGraph;
            GameObject t_obj = GameManager.LoadObj(m_effect_graph, new Vector3(40,57), "Effect/Effect",Vector3.zero,Vector3.one * 103);
            //m_effect_graph.sortingOrder = 0;
            m_anim = t_obj.GetComponent<SpineAnim>();

            onClick.Add(OnClick);
        }

        //初始化宠物数据
        public void InItSingle(PlayerType p_type,FAnimalData p_data, int p_index,EventDelegate p_click_event)
        {
            m_type = p_type;
            m_data = p_data;
            m_click_event = p_click_event;

            m_single.GetIcon.visible = p_data.m_animal_type == FAnimalData.FAnimalType.Null;
            m_group.visible = p_data.m_animal_type != FAnimalData.FAnimalType.Null;

            m_single.SetIcon(p_index + 1);

            m_bg.icon = ComData.GetColor(p_index, "Bg");
            m_power_icon.icon = ComData.GetColor(p_index, "BL");
            m_bar_bg.icon = ComData.GetColor(p_index, "Bg_Bar");
            m_power_bar.value = p_data.m_power_value;
        }

        //更新能量槽
        public void UpdatePowerBar(int p_value)
        {
            m_data.m_power_value += p_value;
            m_power_bar.value = m_data.m_power_value >= m_data.m_power_max_value ?
                m_data.m_power_max_value : (m_data.m_power_value < 0 ? 0 : m_data.m_power_value);

            if(m_data.m_power_value >= m_data.m_power_max_value)
            {
                m_anim.PlayAnim("ui_power", true);
                Debug.Log("Power Up");
            }
        }

        public FAnimalData GetFAnimalData()
        {
            return m_data;
        }

        void OnClick ()
        {
            if (!FightMain.CanClick()) return;
            if (m_type == PlayerType.P_Enemy) return;

            if (m_data.m_power_value >= m_data.m_power_max_value)
            {
                PlaySkill();
            }
        }

        public void PlaySkill()
        {
            m_anim.StopAnim();
            m_data.m_power_value = 0;
            m_power_bar.value = 0;
            m_click_event();
        }

        private bool m_start_attack = false;
        private float m_play_time = 0;
        //播放攻击动画
        public void PlayAttackAnim(bool p_skill, FightEvent p_attack_delegate)
        {
            //Debug.Log("can_attack");
            m_attack = p_attack_delegate;
            m_play_time = 0;
            m_start_attack = true;
        }

        protected override void OnUpdate()
        {
            if (m_start_attack)
            {
                m_play_time += Time.deltaTime;

                if (m_play_time > 1)
                {
                    m_start_attack = false;
                    //Debug.Log("Attack");
                    m_attack();
                }
            }
        }
    }
}
