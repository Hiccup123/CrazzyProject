using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class ShopUI : CommonUI {

    private Dictionary<int, FoodItem> m_foodDic = new Dictionary<int, FoodItem>()
    {
        {0,new FoodItem () { m_icon = "tangguo", m_name = "波彩糖", m_num = 99 }},
        {1,new FoodItem () { m_icon = "bangtang", m_name = "棒棒糖", m_num = 199 }},
        {2,new FoodItem () { m_icon = "buding", m_name = "布丁奇朵", m_num = 299 }},
        {3,new FoodItem () { m_icon = "bangtang", m_name = "棒棒糖", m_num = 99 }},
        {4,new FoodItem () { m_icon = "tangguo", m_name = "波彩糖", m_num = 59 }},
        {5,new FoodItem () { m_icon = "buding", m_name = "布丁奇朵", m_num = 99 }},
    };

    public ShopUI()
    {

    }

    protected override void OnInit()
    {
        base.ComInit("ShopPack","ShopPanel",true, OnClose);

        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("ShopPack", "ShopItem"), typeof(ShopSingle));
        
        OnShown();
    }

    protected override void OnShown()
    {
        base.ComShow("chaoshii");

        GList p_list = this.contentPane.GetChild("ShopList").asList;
        
        p_list.RemoveChildrenToPool();

        p_list.SetVirtual();

        p_list.itemRenderer = RenderListItem;
        p_list.numItems = m_foodDic.Count;

        this.contentPane.GetChild("Btn_Buy").onClick.Add(BuyCallBack);
    }

    void RenderListItem(int p_index,GObject p_obj)
    {
        ShopSingle t_item = p_obj as ShopSingle;
        t_item.InItItem(p_index,m_foodDic[p_index]);
    }

    void BuyCallBack()
    {

    }

    protected override void DoShowAnimation()
    {
        base.ComShowAnimation(OnAnimComplete);
    }

    void OnAnimComplete()
    {

    }

    void OnClose()
    {

    }
}
