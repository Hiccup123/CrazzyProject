using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class CollectUI : Window {

    private GComponent m_bg_obj;
    private GList m_collect_list;
    private GTextField m_num;

    public CollectUI()
    {

    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("PetCollectPack","CollectPanel").asCom;
        this.Center();
        this.modal = true;

        this.OnShown();
    }

    protected override void OnShown()
    {
        m_bg_obj = this.contentPane.GetChild("Bg_Obj").asCom;
        m_bg_obj.GetChild("Btn_Close").onClick.Add(() => { this.HideImmediately(); });

        m_collect_list = this.contentPane.GetChild("AnimalList").asList;
        m_collect_list.RemoveChildrenToPool();

        for (int i = 0; i < 11; i++)
        {
            CollectSingle m_single = m_collect_list.AddItemFromPool() as CollectSingle;
            m_single.InItItem(i < 7);
        }
    } 

    protected override void DoShowAnimation()
    {
        //this.OnShown();
        this.SetScale(0.1f, 0.1f);
        this.SetPivot(0.5f, 0.5f);
        this.TweenScale(new Vector2(1, 1), 0.5f).SetEase(Ease.OutBack,2.5f).OnComplete(OnAnimComplete);
    }

    void OnAnimComplete()
    {

    }

    string NameText(int p_index)
    {
        switch (p_index % 4)
        {
            case 0:
                return "曹凯";
            case 1:
                return "王明曦";
            case 2:
                return "杜果";
            case 3:
                return "苏昆宁";
        }

        return "";
    }
}

class IconData
{
    public bool m_light = true;
    public string m_icon = "";
    public string m_name = "";
}
