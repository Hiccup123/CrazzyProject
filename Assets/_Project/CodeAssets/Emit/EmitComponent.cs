using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class EmitComponent : GComponent {

    private GLoader m_symbol_loader;
    private GTextField m_num_text;
    private GObject m_obj;

    private const float OFFSET_ADDITION = 2.2f;
    private static Vector2 JITTER_FACTOR = new Vector2(40, 40);//方向强度

    public EmitComponent()
    {
        this.touchable = false;

        m_symbol_loader = new GLoader();
        m_symbol_loader.autoSize = true;
        AddChild(m_symbol_loader);

        m_num_text = new GTextField();
        m_num_text.autoSize = AutoSizeType.Both;

        AddChild(m_num_text);
    }

    public void SetNum(GObject p_obj,int p_type,long p_num,bool p_pre_icon)
    {
        m_obj = p_obj;

        TextFormat t_tf = m_num_text.textFormat;
        if(p_type == 0)
        {
            t_tf.font = EmitManager.inst.m_font1;
        }
        else
        {
            t_tf.font = EmitManager.inst.m_font2;
        }
        m_num_text.textFormat = t_tf;
        m_num_text.text = "+" + p_num;

        if(p_pre_icon)
        {
            m_symbol_loader.url = EmitManager.inst.m_pre_icon;
        }
        else
        {
            m_symbol_loader.url = "";
        }

        UpdateLayout();

        this.alpha = 1;
        UpdatePosition(Vector2.zero);
        //Vector2 t_rnd = Vector2.Scale(UnityEngine.Random.insideUnitCircle, JITTER_FACTOR);
        Vector2 t_rnd = Vector2.Scale(Vector2.down, JITTER_FACTOR);
        int t_toX = (int)t_rnd.x * 2;
        int t_toY = (int)t_rnd.y * 2;

        EmitManager.inst.m_view.AddChild(this);
        DOTween.To(() => Vector2.zero, val => { this.UpdatePosition(val); }, new Vector2(t_toX, t_toY), 1f).SetTarget(this).OnComplete(this.OnCompleted);
        this.TweenFade(0, 0.5f).SetDelay(0.5f);
    }

    void UpdateLayout()
    {
        this.SetSize(m_symbol_loader.width + m_num_text.width, Mathf.Max(m_symbol_loader.height, m_num_text.height));
        m_num_text.SetXY(m_symbol_loader.width > 0 ? (m_symbol_loader.width + 2) : 0, (this.height - m_num_text.height) / 2);
        m_symbol_loader.y = (this.height - m_symbol_loader.height) / 2;
    }

    void UpdatePosition(Vector2 p_pos)
    {
        Vector3 t_pos = m_obj.position;
        t_pos.y += OFFSET_ADDITION;
        //Vector3 t_screen_pos = Camera.main.WorldToScreenPoint();
        //Vector3 pt = GRoot.inst.GlobalToLocal(t_screen_pos);
        this.SetXY(Mathf.RoundToInt(m_obj.x + p_pos.x),Mathf.RoundToInt(m_obj.y + p_pos.y + this.height / 2));//x:m_obj.x + p_pos.x - this.actualWidth / 2     y :m_obj.y + p_pos.y - this.height
    }

    void OnCompleted()
    {
        m_obj = null;
        EmitManager.inst.m_view.RemoveChild(this);
        EmitManager.inst.ReturnComponent(this);
    }

    public void Cancel()
    {
        m_obj = null;
        if(this.parent != null)
        {
            DOTween.Kill(this);
            EmitManager.inst.m_view.RemoveChild(this);
        }
        EmitManager.inst.ReturnComponent(this);
    }
}
