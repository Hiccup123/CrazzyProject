using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class PetListSingle : GButton {

    private CollectSingle m_collect_single;
    private GTextField m_name;
    private GTextField m_state;
    private GLoader m_state_icon;
    private GList m_need_list;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_collect_single = GetChild("PetItem") as CollectSingle;
        m_name = GetChild("PetName").asTextField;
        m_state = GetChild("State_text").asTextField;
        m_state_icon = GetChild("State").asLoader;
        m_need_list = GetChild("NeedList").asList;

        this.onClick.Add(CheckPet);
    }

    public void InItItem()
    {
        m_collect_single.InItItem(true);
        m_need_list.RemoveChildrenToPool();
        m_need_list.itemRenderer = ItemRender;
        m_need_list.numItems = 5;
    }

    void ItemRender(int p_index,GObject p_obj)
    {
        //p_obj.asCom.GetChild("n1").icon = UIPackage.GetItemURL ("MainPack","hpg_btn_neizi");
        p_obj.asCom.GetChild("n1").icon = UIPackage.GetItemURL("MainPack", "hpg_btn_" + MainCityUI.m_mainBtnDic[p_index]);
    }

    void CheckPet()
    {
        if (!GameManager.CheckWindow("PetMsgUI"))
        {
            Window m_petMsgPanel = new PetMsgUI();
            GameManager.AddWindowToDic(m_petMsgPanel, "PetMsgUI");
        }

        GameManager.GetWindow("PetMsgUI").Show ();
    }
}
