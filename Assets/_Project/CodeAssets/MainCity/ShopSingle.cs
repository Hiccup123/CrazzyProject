using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class ShopSingle : GComponent {

    private GLoader m_icon;
    private GTextField m_name;
    private GTextField m_attribute;
    private GTextField m_need_money;
    private GLoader m_money_icon;
    private GButton m_add;
    private GButton m_count;
    private GTextField m_buy_num;

    private int m_cur_buy_num;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_icon = GetChild("icon").asLoader;
        m_name = GetChild("FoodName").asTextField;
        m_attribute = GetChild("Attribute").asTextField;
        m_need_money = GetChild("Num").asTextField;
        m_money_icon = GetChild("Cost").asLoader;
        m_buy_num = GetChild("NeedNum").asTextField;
        m_add = GetChild("Btn_Add").asButton;
        m_count = GetChild("Btn_Count").asButton;

        m_add.onClick.Add(ShopItemBtnCallBack);
        m_count.onClick.Add(ShopItemBtnCallBack);

        m_cur_buy_num = 0;
    }

    public void InItItem(int p_index,FoodItem p_item)
    {
        m_icon.icon = UIPackage.GetItemURL("FoodPack", "food_text_" + p_item.m_icon);
        m_name.text = p_item.m_name;
        m_attribute.text = p_index / 2 == 0 ? "魅力+19" : "武力+1";
        m_money_icon.icon = UIPackage.GetItemURL("ComPack", "combg_text_" + (p_index / 2 == 0 ? "jinbi" : "zuanshi"));
        m_need_money.text = Random.Range(599, 999).ToString();
        SetBuyNum(m_cur_buy_num);
    }

    void ShopItemBtnCallBack(EventContext p_event_context)
    {
        string p_name = ((GObject)(p_event_context.sender)).name.Substring(4);

        switch (p_name)
        {
            case "Add":

                m_cur_buy_num++;

                break;
            case "Count":

                m_cur_buy_num = m_cur_buy_num - 1 <= 0 ? 0 : m_cur_buy_num - 1;

                break;
        }

        SetBuyNum(m_cur_buy_num);
    }

    void SetBuyNum(int p_num)
    {
        m_buy_num.text = p_num.ToString ();
    }
}
