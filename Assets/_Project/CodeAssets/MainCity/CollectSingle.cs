using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class CollectSingle : GComponent {

    private GTextField m_title;
    private GLoader m_bg;
    private GLoader m_icon;
    private GLoader m_vip_box;
    private GLoader m_vip;
    private GComponent m_zhezhao;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_title = GetChild("BgObj").asCom.GetChild("n2").asTextField;
        m_bg = GetChild("BgObj").asCom.GetChild("Bg").asLoader;
        m_icon = GetChild("Icon").asLoader;
        m_vip_box = GetChild("VipBox").asLoader;
        m_vip = GetChild("VipIcon").asLoader;
        m_zhezhao = GetChild("ZheZhao").asCom;
    }

    public void InItItem(bool p_have)
    {
        m_title.text = "喜洋洋";
        m_icon.visible = p_have;
        m_vip_box.visible = p_have;
        m_vip.visible = p_have;
        m_zhezhao.visible = !p_have;
    }
}
