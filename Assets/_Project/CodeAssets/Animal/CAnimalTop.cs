using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class CAnimalTop : GComponent {

    private BaseData m_data;

    private Controller m_controller;
    private GProgressBar m_hunger_progress;
    private GMovieClip m_hand;

    private GComponent m_qipao;
    private GLoader m_qipao_icon;
    private Transition m_transition;

    //0-食物 1-抚摸 2-洗澡 3-生病
    private string[] m_begStr = new string[] {"shiwu", "fumo", "xizao", "beibao"};

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        m_controller = this.GetController("State");
        m_hunger_progress = this.GetChild("Hunger").asProgress;
        m_hand = GetChild("Caught").asMovieClip;
      
        m_qipao = GetChild("QiPao").asCom;
        m_qipao_icon = m_qipao.GetChild("Icon").asLoader;
        m_transition = m_qipao.GetTransition("t0");
    }

    public void InItComponent(BaseData p_data)
    {
        m_data = p_data;
        SetHand(false);
        SetQiPao(false);
    }

    public void SetComponent(ControlState p_state)
    {
        m_controller.selectedIndex = (int)p_state;

        if (p_state == ControlState.Food)
        {
            SetHunger(m_data.m_hunger);
            //bool t_hunger = m_data.m_hunger < 100;
            //m_hunger_progress.visible = t_hunger;
            //if (t_hunger)
            //{
            //    SetHunger(m_data.m_hunger);
            //}
            //else
            //{

            //}
        }
        else if(p_state == ControlState.FuMo)
        {
            SetHunger(m_data.m_appease);
        }
        else if(p_state == ControlState.Shower)
        {
            SetHunger(m_data.m_clean);
        }
        else if(p_state == ControlState.ZhiLiao)
        {
            SetHunger(m_data.m_health);
        }
    }

    void SetHunger(float p_value)
    {
        p_value = (int)(p_value / 20) * 20;
        m_hunger_progress.value = p_value;
        m_hunger_progress.GetChild("Light").visible = p_value != 0 && p_value != 100;
        m_hunger_progress.GetChild("bar").asCom.GetChild("icon").icon = UIPackage.GetItemURL("FoodPack", "food_text_shiwutiao1");//UIPackage.GetItemURL("MainPack", "bathe_text_xizao1");
    }

    public void SetHand (bool p_visible)
    {
        m_hand.visible = p_visible;
        m_hand.playing = p_visible;
        m_hand.asMovieClip.SetPlaySettings(0, -1, 1, -1);
    }

    public void SetQiPao(bool p_visible,int p_show_type = -1)
    {
        m_qipao.visible = p_visible && p_show_type != -1;
        if(!p_visible)
        {
            return;
        }
        m_transition.Play();
        string t_hpg = "";
        t_hpg = m_begStr[p_show_type];
        
        m_qipao_icon.icon = UIPackage.GetItemURL("MainPack", "hpg_btn_" + t_hpg);
    }

    public void SetHandPos(bool p_left)
    {
        GetChild("Caught").position = new Vector3(p_left ? 0 : 40, GetChild("Caught").position.y, GetChild("Caught").position.z);
    }
}
