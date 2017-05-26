using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class CommonUI : Window {

    private GLoader m_title;

    private GButton m_close;

    public delegate void CloseDelegate ();
    public CloseDelegate m_close_delegate;

    virtual protected void ComInit(string p_pack_name,string p_ui_name,bool p_modal,CloseDelegate p_delegate = null)
    {
        this.contentPane = UIPackage.CreateObject(p_pack_name,p_ui_name).asCom;
        this.Center();
        this.modal = p_modal;

        m_close_delegate = p_delegate;
    }

    virtual protected void ComShow(string p_title_name = "")
    {
        if (!string.IsNullOrEmpty (p_title_name))
        {
            m_title = this.contentPane.GetChild("Title").asLoader;
            m_title.icon = UIPackage.GetItemURL("ComPack", "title_text_" + p_title_name);
        }
        m_close = this.contentPane.GetChild("Btn_Close").asButton;
        
        m_close.onClick.Add(() => 
        {
            if (m_close_delegate != null)
            {
                m_close_delegate();
            }
            this.HideImmediately ();
        });
    }

    virtual protected void ComShowAnimation(TweenCallback p_call_back = null)
    {
        this.SetScale(0.1f, 0.1f);
        this.SetPivot(0.5f, 0.5f);
        this.TweenScale(new Vector2(1, 1), 0.5f).SetEase(Ease.OutBack, 2.5f).OnComplete(p_call_back);
    }
}
