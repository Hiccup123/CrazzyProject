using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class CFoodSingle : GComponent {

    private GLoader m_icon;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_icon = GetChild("Icon").asLoader;
    }

    public void InItFood (string p_name)
    {
        m_icon.icon = UIPackage.GetItemURL("FoodPack","food_text_" + p_name);
    }

    public void RemoveFood ()
    {
        Tweener t_tweener = m_icon.TweenFade(0, 0.2f);
        t_tweener.SetEase(Ease.Linear);
        t_tweener.OnComplete(() =>
        {
            AnimalManager.m_instance.RemoveFood(this);
        });
    }
}
