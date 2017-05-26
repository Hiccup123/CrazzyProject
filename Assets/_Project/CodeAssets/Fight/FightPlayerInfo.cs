using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

namespace Fight
{
    public class FightPlayerInfo : GComponent
    {

        private GProgressBar m_blood_bar;
        private GList m_animal_list;
        private GLoader m_bg_loader;
        private GLoader m_huawen;
        private GGroup m_text_group;

        private GComponent m_player;
        private GLoader m_head;

        private GTextField m_name;
        private GTextField m_state;

        private List<FAnimalSingle> m_f_animal_list = new List<FAnimalSingle>();
        private FightData m_data;

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_blood_bar = GetChild("Blood").asProgress;
            m_animal_list = GetChild("List").asList;
            m_bg_loader = GetChild("Bg").asLoader;
            m_huawen = GetChild("HuaWen").asLoader;
            m_text_group = GetChild("TextGroup").asGroup;

            m_player = GetChild("Player").asCom;
            m_head = m_player.GetChild("PlayerHead").asLoader;

            m_name = GetChild("NameText").asTextField;
            m_state = GetChild("StateText").asTextField;

            m_blood_bar.titleType = ProgressTitleType.ValueAndMax;
        }

        public void InItPlayerInfo(PlayerType p_type, FightData p_data,EventDelegate p_click_event)
        {
            m_data = p_data;
            m_blood_bar.max = m_data.m_blood;

            switch (p_type)
            {
                case PlayerType.P_Enemy:

                    m_bg_loader.rotation = 180;
                    m_bg_loader.position = Vector3.zero;
                    m_blood_bar.position = new Vector3(11, 122, 0);
                    m_animal_list.position = new Vector3(9, 3, 0);
                    m_player.position = new Vector3(520, 2, 0);
                    m_text_group.position = new Vector3(389, 14);
                    m_huawen.rotation = 180;
                    m_huawen.position = new Vector3(0, 0);

                    break;
                case PlayerType.P_Self:

                    m_bg_loader.rotation = 0;
                    m_bg_loader.position = new Vector3(0, 29, 0);
                    m_blood_bar.position = new Vector3(11, 1, 0);
                    m_animal_list.position = new Vector3(9, 38, 0);
                    m_player.position = new Vector3(520, 6, 0);
                    m_text_group.position = new Vector3(389, 51);
                    m_huawen.rotation = 0;
                    m_huawen.position = new Vector3(0, 35);

                    break;
            }

            //m_head.icon = UIPackage.GetItemURL("FightPack", p_data.m_icon);
            m_name.text = p_data.m_name;

            SetBloodValue(0);

            m_animal_list.RemoveChildrenToPool();
            for (int i = 0; i < 4; i++)
            {
                FAnimalSingle t_item = m_animal_list.AddItemFromPool() as FAnimalSingle;
                t_item.InItSingle(p_type,i < p_data.m_data_list.Count ? p_data.m_data_list[i] : new FAnimalData(), i, p_click_event);
                if (i < p_data.m_data_list.Count)
                {
                    m_f_animal_list.Add(t_item);
                }
            }
        }

        //设置血条  p_count_value : <0:减血  >0:加血
        public void SetBloodValue(float p_count_value)
        {
            //Debug.Log("p_count_value:" + p_count_value);
            //Debug.Log("m_data.m_blood:" + m_data.m_blood);
            m_data.m_blood = p_count_value < 0 ? (m_data.m_blood + p_count_value > 0 ? m_data.m_blood + p_count_value : 0) : (m_data.m_blood + p_count_value > m_blood_bar.max ? (float)m_blood_bar.max : m_data.m_blood + p_count_value);
            m_blood_bar.value = m_data.m_blood;
        }

        //设置行动状态
        public void SetStateText(int p_count)
        {
            m_state.text = p_count == -1 ? "待机中..." : "剩余：" + p_count + "回合";
        }

        //指定宠物攻击
        public void Attack(int p_id, bool p_skill, FightEvent p_attack_delegate, Attack p_end)
        {
            //Debug.Log("m_animal_list.lineItemCount:" + m_animal_list.GetChildren().Length);
            bool t_null_id = true;
            for (int i = 0; i < m_animal_list.GetChildren().Length; i++)
            {
                if (i == p_id - 1)
                {
                    FAnimalSingle t_single = m_animal_list.GetChildAt(i) as FAnimalSingle;
                    //Debug.Log("m_animal_type:" + t_single.GetFAnimalData().m_animal_type);
                    if (t_single.GetFAnimalData().m_animal_type != FAnimalData.FAnimalType.Null)
                    {
                        //Debug.Log("m_animal_type:" + t_single.GetFAnimalData().m_animal_type);
                        t_null_id = false;
                        t_single.PlayAttackAnim(p_skill, p_attack_delegate);
                    }
                }
            }

            if (t_null_id)
            {
                p_end();
            }
        }

        //更新能量槽
        public void UpdatePowerBar(int p_id, int p_value)
        {
            for (int i = 0; i < m_animal_list.GetChildren().Length; i++)
            {
                if (i == p_id - 1)
                {
                    FAnimalSingle t_single = m_animal_list.GetChildAt(i) as FAnimalSingle;
                    if (t_single.GetFAnimalData().m_animal_type != FAnimalData.FAnimalType.Null)
                    {
                        t_single.UpdatePowerBar(p_value);
                    }
                }
            }
        }

        //检测是否可释放技能
        public bool CheckPowerUp ()
        {
            foreach (var item in m_f_animal_list)
            {
                if (item.GetFAnimalData().m_power_value >= item.GetFAnimalData().m_power_max_value)
                {
                    return true;
                }
            }

            return false;
        }

        //自动释放技能
        public void AutoSkill ()
        {
            foreach (var item in m_f_animal_list)
            {
                if(item.GetFAnimalData().m_power_value >= item.GetFAnimalData().m_power_max_value)
                {
                    item.PlaySkill();
                    break;
                }
            }
        }

        //检测是否死亡
        public bool Dead()
        {
            return m_blood_bar.value <= 0;
        }
    }
}

