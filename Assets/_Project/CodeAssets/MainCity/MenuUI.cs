using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class MenuUI : CommonUI
{
    private GTextField m_text_name;
    private GList m_btn_list;

    private readonly Dictionary<int, string> m_btnDic = new Dictionary<int, string>()
    {
        {0, "paihang"},{1, "chakan"},{2, "tujian"},
        { 3, "shangcheng"},{4, "chengbao"},{5, "renwu"},
        { 6, "haoyou"},{7, "xiaoxi"},{8, "shezhi"},
    };

    public MenuUI()
    {

    }

    protected override void OnInit()
    {
        base.ComInit("MenuPack","MenuPanel",true, OnClose);
        m_text_name = this.contentPane.GetChild("Name").asTextField;
        m_btn_list = this.contentPane.GetChild("Btn_List").asList;
        OnShown();
    }

    protected override void OnShown()
    {
        base.ComShow();
        m_text_name.text = "曹凯";

        //Debug.Log("p_list.lineItemCount：" + p_list.lineItemCount);
        m_btn_list.RemoveChildrenToPool();

        for (int i = 0; i < 9; i++)
        {
            GButton t_item = m_btn_list.AddItemFromPool().asButton;
            
            t_item.name = "MenuBtn_" + i;

            t_item.icon = UIPackage.GetItemURL("MenuPack", "menu_btn_" + m_btnDic[i]);

            t_item.onClick.Add(MeunBtnEventCallBack);
        }
    }

    void MeunBtnEventCallBack(EventContext p_event_context)
    {
        string p_name = ((GObject)(p_event_context.sender)).name.Substring (8);
        Debug.Log("p_name:" + p_name);

        switch (int.Parse (p_name))
        {
            case 0://排行榜
                break;
            case 1://查看宠物
                GameManager.OpenWindow("PetListUI", new PetListUI());
                break;
            case 2://宠物图鉴
                GameManager.OpenWindow("CollectUI", new CollectUI());
                break;
            case 3://商城
                GameManager.OpenWindow("ShopUI", new ShopUI());
                break;
            case 4://城堡
                break;
            case 5://活动任务
                break;
            case 6://社交
                break;
            case 7://消息
                break;
            case 8://设置
                GameManager.OpenWindow("SetUI", new SetUI());
                break;
            default:
                break;
        }
    }

    protected override void DoShowAnimation()
    {
        base.ComShowAnimation (OnAnimComplete);
    }

    void OnAnimComplete()
    {

    }

    void OnClose()
    {

    }
}
