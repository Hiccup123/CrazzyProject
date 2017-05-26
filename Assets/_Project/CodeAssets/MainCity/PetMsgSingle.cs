using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

public class PetMsgSingle : GButton {

    private MsgType m_msg_type;

    private GGroup m_show_group;

    private GComponent m_msg_single;
    private GButton m_btn_check;
    private GLoader m_single_icon;

    private GTextField m_name;

    private GButton m_btn_left;
    private GButton m_btn_right;

    private GObject m_obj_add;
    private bool m_empty = false;

    public override void ConstructFromXML(XML cxml)
    {
        base.ConstructFromXML(cxml);

        m_show_group = GetChild("ShowItem").asGroup;
       
        m_msg_single = GetChild("Item").asCom;
        m_btn_check = m_msg_single.GetChild("Btn_Check").asButton;
        m_single_icon = m_msg_single.GetChild("Icon").asLoader;

        m_name = GetChild("ItemName").asTextField;
        m_btn_left = GetChild("Btn_Left").asButton;
        m_btn_right = GetChild("Btn_Right").asButton;
        m_obj_add = GetChild("Add");

        this.onClick.Add(OnClickBack);
        m_btn_left.onClick.Add(OnClickBack);
        m_btn_right.onClick.Add(OnClickBack);
        m_btn_check.onClick.Add(OnClickBack);
    }

    #region PetSkillMsg
    public void SetMsgType(MsgType p_type,bool p_empty)
    {
        m_msg_type = p_type;

        m_empty = p_empty;

        m_show_group.visible = !p_empty;
        m_obj_add.visible = p_empty;

        m_btn_check.visible = p_type != MsgType.Skill;

        switch (p_type)
        {
            case MsgType.Skill:
                m_btn_left.title = "详情";
                m_btn_right.title = "遗忘";
                break;
            case MsgType.Equipment:
                m_btn_left.title = "装备";
                m_btn_right.title = "卸下";
                break;
            case MsgType.ShiPin:
                m_btn_left.title = "升级";
                m_btn_right.title = "卸下";
                break;
        }
    }

    void OnClickBack(EventContext p_event_context)
    {
        string t_name = ((GObject)(p_event_context.sender)).name;
        switch (t_name)
        {
            case "Btn_Left":
                switch (m_msg_type)
                {
                    case MsgType.Skill:
                        break;
                    case MsgType.Equipment:
                        break;
                    case MsgType.ShiPin:
                        break;
                }
                break;
            case "Btn_Right":
                switch (m_msg_type)
                {
                    case MsgType.Skill:
                        break;
                    case MsgType.Equipment:
                        break;
                    case MsgType.ShiPin:
                        break;
                }
                break;
            case "Btn_Check":
                switch (m_msg_type)
                {
                    //case MsgType.Skill:
                    //    break;
                    case MsgType.Equipment:
                        break;
                    case MsgType.ShiPin:
                        break;
                }
                break;
            default:
                if (m_empty)
                {
                    switch (m_msg_type)
                    {
                        case MsgType.Skill:
                            Debug.Log("New Skill");
                            break;
                        case MsgType.Equipment:
                            Debug.Log("New Equip");
                            break;
                        case MsgType.ShiPin:
                            Debug.Log("New ShiPin");
                            break;
                    }
                }
                break;
        }
    }
    #endregion

    #region FightEndReward
    public void SetReward()
    {

    }
    #endregion
}
