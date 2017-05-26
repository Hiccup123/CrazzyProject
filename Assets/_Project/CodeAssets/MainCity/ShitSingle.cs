using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class ShitSingle : GComponent {

    private GLoader m_icon;

    private float m_touch_time = 0;

    private bool m_touching = false;

    private bool m_can_clean = false;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_icon = GetChild("Icon").asLoader;

        onTouchBegin.Add(TouchBegin);
        onTouchEnd.Add(TouchEnd);
    }
    
    public void SetShitTouchAble (bool p_value)
    {
        m_can_clean = p_value;
    }

    void TouchBegin()
    {
        m_touching = true;  
    }

    protected override void OnUpdate()
    {
        if (!m_can_clean) return;

        if(m_touching)
        {
            m_touch_time += Time.deltaTime;
            if (m_touch_time > 1)
            {
                if (m_touching)
                {
                    m_touching = false;
                    m_touch_time = 0;
                    Tweener t_tweener = m_icon.TweenFade(0,0.5f);
                    t_tweener.SetEase(Ease.Linear);
                    t_tweener.OnComplete(() => 
                    {
                        Debug.Log("Remove");
                        AnimalManager.m_instance.RemoveShit(this);
                    });
                }     
            }
        }
    }

    void TouchEnd ()
    {
        m_touching = false;
        m_touch_time = 0;
    }
}
