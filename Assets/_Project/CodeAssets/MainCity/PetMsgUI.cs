using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System.Linq;

public enum MsgType
{
    Skill = 0,          //技能
    Equipment = 1,      //装备
    ShiPin = 2,         //饰品
}

public class PetMsgUI : CommonUI {

    private PetMsgJiBen m_page_jiben;
    private GList m_pet_msg_list;
    private List<GButton> m_page_btn_list = new List<GButton>();

    private Controller m_page_controller;

    private string m_cur_btn_name;

    public PetMsgUI()
    {
        
    }

    protected override void OnInit()
    {
        base.ComInit("PetMsgPack","PetMsgUI",true, OnClose);

        m_page_jiben = this.contentPane.GetChild("Page_JiBen") as PetMsgJiBen;
        m_pet_msg_list = this.contentPane.GetChild("PetMsgList").asList;

        m_page_controller = this.contentPane.GetController("Page");

        for (int i = 0;i < this.contentPane.numChildren;i++)
        {
            string p_obj_name = this.contentPane.GetChildAt(i).name;
            //Debug.Log("p_obj_name:" + p_obj_name);
            if (p_obj_name.Length < 9)
            {
                continue;
            }
            if (this.contentPane.GetChildAt (i).name.Substring (0,9) == "Btn_Page_")
            {
                m_page_btn_list.Add(this.contentPane.GetChildAt (i).asButton);
            }
        }

        base.ComShow();
        m_page_btn_list.ForEach(item => { item.onClick.Add(PageBtnCallBack); });
    }

    protected override void OnShown()
    {
        SetPage("JiBen");
    }

    void PageBtnCallBack(EventContext p_event_context)
    {
        string t_btn_name = ((GObject)(p_event_context.sender)).name;
        Debug.Log("t_btn_name:" + t_btn_name);

        string t_name = t_btn_name.Substring(9);
        Debug.Log("t_name:" + t_name);
        if (t_name == m_cur_btn_name)
        {
            return;
        }
        m_cur_btn_name = t_name;

        SetPage(t_name);        
    }

    void SetPage(string p_name)
    {
        m_page_controller.selectedIndex = p_name == "JiBen" ? 0 : 1;

        m_page_btn_list.ForEach(item => 
        {
            item.GetController("Color").selectedIndex = item.name.Substring (9) == p_name ? 0 : 1;
        });

        if (p_name == "JiBen")
        {
            Debug.Log("m_page_jiben:" + m_page_jiben);
            m_page_jiben.InItPage();
        }
        else
        {
            MsgType t_type = MsgType.Skill;
            switch (p_name)
            {
                case "JiNeng":
                    t_type = MsgType.Skill;
                    break;
                case "ZhuangBei":
                    t_type = MsgType.Equipment;
                    break;
                case "ShiPin":
                    t_type = MsgType.ShiPin;
                    break;
            }

            m_pet_msg_list.RemoveChildrenToPool();
            
            for (int i = 0;i < 3;i ++)
            {
                PetMsgSingle t_single = m_pet_msg_list.AddItemFromPool() as PetMsgSingle;

                t_single.SetMsgType(t_type, i > 1);
            }
        }  
    }

    protected override void DoShowAnimation()
    {
        OnShown();
        base.ComShowAnimation(OnAnimComplete);
    }

    void OnAnimComplete()
    {

    }

    void OnClose()
    {
        m_cur_btn_name = "";
    }
}
