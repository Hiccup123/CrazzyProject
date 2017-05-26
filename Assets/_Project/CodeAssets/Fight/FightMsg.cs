using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;
using Spine.Unity;

namespace Fight
{
    public class FightMsg : GComponent
    {
        private FightPlayerInfo m_self_info;
        private FightPlayerInfo m_enemy_info;
        private GComponent m_zhezhao;
        private Transition m_trans;
        private GLoader m_black;

        private SpineAnim m_warning;

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            var t_self = GetChild("Bottom").asCom;
            m_self_info = t_self as FightPlayerInfo;

            var t_enemy = GetChild("Top").asCom;
            m_enemy_info = t_enemy as FightPlayerInfo;

            m_black = GetChild("Black").asLoader;

            m_trans = GetTransition("t0");

            m_zhezhao = GetChild("ZheZhao").asCom;
            GGraph t_graph = m_zhezhao.GetChild("Effect").asGraph;
            GameObject t_obj = GameManager.LoadObj(t_graph, new Vector3(286,336), "Effect/Effect", Vector3.zero, new Vector3(89,97,1));
            m_warning = t_obj.GetComponent<SpineAnim>();
        }

        //初始化玩家信息
        public void SetPlayerInfo(PlayerType p_type, FightData p_data,EventDelegate p_click_event)
        {
            switch (p_type)
            {
                case PlayerType.P_Self:
                    m_self_info.InItPlayerInfo(PlayerType.P_Self, p_data, p_click_event);
                    break;
                case PlayerType.P_Enemy:
                    m_enemy_info.InItPlayerInfo(PlayerType.P_Enemy, p_data, p_click_event);
                    break;
            }
        }

        //更新血条
        public void UpdateBlood(PlayerType p_type, float p_count_value)
        {
            switch (p_type)
            {
                case PlayerType.P_Self:
                    m_self_info.SetBloodValue(p_count_value);
                    SelfBeAttack();
                    break;
                case PlayerType.P_Enemy:
                    m_enemy_info.SetBloodValue(p_count_value);
                    break;
            }
        }

        //更新状态
        public void UpdateStateText(PlayerType p_type, int p_count)
        {
            switch (p_type)
            {
                case PlayerType.P_Self:
                    m_self_info.SetStateText(p_count);
                    break;
                case PlayerType.P_Enemy:
                    m_enemy_info.SetStateText(p_count);
                    break;
            }
        }

        //更新能量槽
        public void UpdatePowerBar(PlayerType p_type, int p_id, int p_value)
        {
            switch (p_type)
            {
                case PlayerType.P_Self:
                    m_self_info.UpdatePowerBar(p_id, p_value);
                    break;
                case PlayerType.P_Enemy:
                    m_enemy_info.UpdatePowerBar(p_id, p_value);
                    break;
            }
        }

        //攻击
        public void PlayAttack(PlayerType p_type, int p_id, bool p_skill, FightEvent p_delegate, Attack p_end)
        {
            //Debug.Log("p_id:" + p_id);
            switch (p_type)
            {
                case PlayerType.P_Self:
                    m_self_info.Attack(p_id, p_skill, p_delegate, p_end);
                    break;
                case PlayerType.P_Enemy:
                    m_enemy_info.Attack(p_id, p_skill, p_delegate, p_end);
                    break;
            }
        }

        //切换回合提示
        public void ChangeTurn(bool p_visible, PlayCompleteCallback p_on_end_call_back)
        {
            //m_zhezhao.visible = p_visible;
            m_black.visible = p_visible;
            if (p_visible)
            {
                m_trans.Play(p_on_end_call_back);
            }
        }

        //被攻击提示动画
        public void SelfBeAttack()
        {
            m_warning.SetEvent("end", () => {  });
            m_warning.PlayAnim("hurt_light", false);
        }

        //检测是否可释放技能
        public bool CheckAutoSkill(PlayerType p_type)
        {
            switch (p_type)
            {
                case PlayerType.P_Self:
                    return m_self_info.CheckPowerUp();
                case PlayerType.P_Enemy:
                    return m_enemy_info.CheckPowerUp();
            }
            return false;
        }

        //自动释放技能
        public void AutoSkill(PlayerType p_type)
        {
            switch (p_type)
            {
                case PlayerType.P_Self:
                    m_self_info.AutoSkill();
                    break;
                case PlayerType.P_Enemy:
                    m_enemy_info.AutoSkill();
                    break;
            }
        }

        //检测是否死亡
        public bool CheckDead(PlayerType p_type)
        {
            return p_type == PlayerType.P_Self ? m_self_info.Dead() : m_enemy_info.Dead();
        }

        //显示战斗结束界面
        public void ShowEndUI(PlayerType p_type)
        {
            //战斗结束
            FightEndMsg t_end_msg = new FightEndMsg();
            t_end_msg.m_end_id = p_type == PlayerType.P_Self ? 1 : 0;
            t_end_msg.m_stars = 3;
            t_end_msg.m_player_id = 0;
            t_end_msg.m_name = p_type == PlayerType.P_Self ? "Hiccup" : "Boss";
            t_end_msg.m_exp = 998;
            t_end_msg.m_level = 9;
            t_end_msg.m_reward_list = new List<int>(8);
            GameManager.OpenWindow("FightEndUI", new FightEndUI(t_end_msg));
        }
    }
}

