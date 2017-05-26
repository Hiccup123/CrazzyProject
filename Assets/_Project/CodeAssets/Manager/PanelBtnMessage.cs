using UnityEngine;
using System.Collections;

public class PanelBtnMessage : MonoBehaviour {

	public PanelBase m_panel;
	bool m_Started = false;
	bool m_Highlighted = false;

	void Start () 
	{
		m_Started = true; 
	}
	
	void OnEnable ()
	{
		if (m_Started && m_Highlighted)
		{
//			OnHover (UICamera.IsHighlighted (gameObject));
		}
	}
	
	public void OnHover (bool isOver)
	{
		if (enabled)
		{
			m_Highlighted = isOver;
		}
		else
		{
			
		}
	}
	
	public void OnPress (bool isPressed)
	{
		if (enabled)
		{
			m_panel.MYPress (isPressed, gameObject);
		}
	}
	
	public void OnClick ()
	{
		float tempTime = Time.time;
		if (tempTime - m_panel.m_TimeP > 0.2f)
		{
			m_panel.m_TimeP = tempTime;
			m_panel.MYClick (gameObject);
		}
	}
	
	public void OnDoubleClick ()
	{
		if (enabled )
		{
			m_panel.MYoubleClick (gameObject);
		}
	}
	
	public void OnDrag(Vector2 delta)
	{
		m_panel.MYondrag (delta);
	}
	
	public void OnInput (string text)
	{
		m_panel.MYonInput (gameObject, text);
	}
}
