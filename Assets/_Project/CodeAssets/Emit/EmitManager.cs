using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class EmitManager
{
    private static EmitManager m_instance;
    public static EmitManager inst
    {
        get
        {
            if(m_instance == null)
                m_instance = new EmitManager();

            return m_instance;
        }
    }

    public string m_font1;
    public string m_font2;
    public string m_pre_icon;

    public GComponent m_view { get; private set; }

    private readonly Stack<EmitComponent> m_component_pool = new Stack<EmitComponent>();

    public EmitManager()
    {
        m_font1 = UIPackage.GetItemURL("EmitNumbersPack", "number1");
        m_font1 = UIPackage.GetItemURL("EmitNumbersPack", "number2");
        m_pre_icon = UIPackage.GetItemURL("EmitNumbersPack", "critical");

        m_view = new GComponent();
        GRoot.inst.AddChild(m_view);
    }

    public void Emit(GObject p_obj,int p_type,long p_num,bool p_pre_icon)
    {
        EmitComponent t_ec;
        if(m_component_pool.Count > 0)
        {
            t_ec = m_component_pool.Pop();
        }
        else
        {
            t_ec = new EmitComponent();
        }
        t_ec.SetNum(p_obj, p_type, p_num, p_pre_icon);
    }

    public void ReturnComponent(EmitComponent p_ec)
    {
        m_component_pool.Push(p_ec);
    }
}
