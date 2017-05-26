using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System.Linq;

public class FoodUI : Window {

    private Dictionary<int, FoodItem> m_foodDic = new Dictionary<int, FoodItem>()
    {
        {0,new FoodItem () { m_icon = "tangguo", m_name = "波彩糖", m_num = 99 }},
        {1,new FoodItem () { m_icon = "bangtang", m_name = "棒棒糖", m_num = 99 }},
        {2,new FoodItem () { m_icon = "buding", m_name = "布丁奇朵", m_num = 99 }},
        {3,new FoodItem () { m_icon = "jia", m_name = "", m_num = 0 }},
    };

    public FoodUI()
    {

    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("FoodPack", "FoodUI").asCom;
        this.Center();
        //this.modal = true;
        
        OnShown();
    }

    protected override void OnShown()
    {
        GList m_foodList = this.contentPane.GetChild("FoodList").asList;
        m_foodList.RemoveChildrenToPool();
        int m_foodCount = 4;
        for (int i = 0;i < m_foodCount;i ++)
        {
            FoodSingle p_food_single = m_foodList.AddItemFromPool() as FoodSingle;
            p_food_single.InItItem(m_foodDic[i],i,m_foodCount);
        }
    }

    protected override void DoShowAnimation()
    {
        
    }
}

public class FoodItem
{
    public string m_name;
    public string m_icon;
    public int m_num;
}
