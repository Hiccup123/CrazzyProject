using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

public class PetMsgJiBen : GComponent {

    private CollectSingle m_collect_single;
    private GTextField m_name;
    private GButton m_btn_change;
    private GLoader m_sex_icon;
    private GTextField m_sex_text;
    private GLoader m_health_icon;
    private GTextField m_loyal;

    private GComponent m_at1;
    private GComponent m_at2;
    private GComponent m_at3;

    private GProgressBar m_grow_bar;

    private readonly string[] m_health_length = new string[] { "hong","huang","lv" };

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_collect_single = GetChild("PetItem") as CollectSingle;
        m_name = GetChild("PetName").asTextField;
        m_btn_change = GetChild("Btn_Change").asButton;
        m_sex_icon = GetChild("SexIcon").asLoader;
        m_sex_text = GetChild("Age").asTextField;
        m_health_icon = GetChild("HealthyIcon").asLoader;
        m_loyal = GetChild("LoyalNum").asTextField;

        m_at1 = GetChild("At_Item1").asCom;
        m_at2 = GetChild("At_Item2").asCom;
        m_at3 = GetChild("At_Item3").asCom;

        m_grow_bar = GetChild("Grow_Bar").asProgress;
        m_btn_change.onClick.Add(ChangeName);

        SetAttribute(1, 100, 2);
        SetAttribute(2, 200, 1);
        SetAttribute(3, 400, 4);

        SetGrowBar(60);
    }

    public void InItPage()
    {
        m_collect_single.InItItem(true);
        m_health_icon.icon = UIPackage.GetItemURL("PetCollectPack", "msg_text_" + m_health_length[0]);
    }

    void ChangeName()
    {
        Debug.Log("ChangeBtn");
    }

    public void SetAttribute(int p_index,int p_value,int p_bar_value)
    {
        switch (p_index)
        {
            case 1:
                SetAttributeBase(m_at1,"生命",p_value.ToString (),"饥饿",p_bar_value);
                break;
            case 2:
                SetAttributeBase(m_at2, "武力", p_value.ToString(), "卫生", p_bar_value);
                break;
            case 3:
                SetAttributeBase(m_at3, "魅力", p_value.ToString(), "安抚", p_bar_value);
                break;
        }
    }

    void SetAttributeBase(GComponent p_com,string p_name1,string p_value,string p_name2,int p_var_value)
    {
        p_com.GetChild("At_Name1").text = p_name1;
        p_com.GetChild("At_Num").text = p_value;
        p_com.GetChild("At_Name2").text = p_name2;
        
        if (p_var_value > 4 || p_var_value < 0)
        {
            Debug.LogError("Error value:" + p_var_value);
            return;
        }
        GComponent p_star_bar = p_com.GetChild("At_Bar").asCom;
        for (int i = 0; i < 5; i++)
        {
            GComponent t_star = p_star_bar.GetChildAt(i).asCom;
            Controller t_star_controller = t_star.GetController("Show");
            t_star_controller.selectedIndex = i < p_var_value ? 0 : 1;
        }
    }

    public void SetGrowBar(int p_value)
    {
        m_grow_bar.value = p_value;
    }
}
