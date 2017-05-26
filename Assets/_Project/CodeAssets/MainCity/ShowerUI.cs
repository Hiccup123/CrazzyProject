using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class ShowerUI : GComponent {

    private GComponent m_shower_head;
    private GObject m_shower_line;
    private Vector3 m_root_pos;
    private float m_dis;

    private bool m_touching = false;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        m_shower_head = GetChild("ShowerHead").asCom;
        m_shower_line = GetChild("Line");
    }

    public void InItShowerUI (Rect p_rect)
    {
        m_shower_head.draggable = true;
        m_shower_head.dragBounds = p_rect;
        m_shower_head.onTouchBegin.Add(TouchBegin);
        m_shower_head.onTouchEnd.Add(TouchEnd);
        m_root_pos = new Vector3(270, -50);
        m_touching = false;
        SetShowerVisable(false);
        m_shower_head.sortingOrder = 10000;
        m_shower_line.sortingOrder = 10000;
    }

    public void SetShowerVisable (bool p_value)
    {
        if (p_value)
        {
            m_shower_head.position = new Vector3(270,-50);
            m_shower_line.position = new Vector3(312, -110);
        }
        m_shower_head.visible = p_value;
        m_shower_line.visible = p_value;
    }

    void TouchBegin()
    {
        m_touching = true;
    }
    void TouchEnd()
    {
        m_touching = false;
        Debug.Log("touchend");
        Tweener t_tweener = m_shower_head.TweenMove(m_root_pos, 1);
        t_tweener.SetEase(Ease.OutBack);
    }

    protected override void OnUpdate()
    {
        m_shower_line.position = new Vector3(m_shower_head.position.x + 57, m_shower_line.position.y);

        if (m_touching)
        {
            AnimalManager.m_instance.FindShowerTarget(m_shower_head);
        }
    }
}
