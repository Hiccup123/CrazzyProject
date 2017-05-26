using UnityEngine;
using System.Collections;

public abstract class PanelBase : MonoBehaviour {

	public float m_TimeP = 0;
	public abstract void MYClick(GameObject ui);
	public abstract void MYMouseOver(GameObject ui);
	public abstract void MYMouseOut(GameObject ui);
	public abstract void MYPress(bool isPress, GameObject ui);
	public abstract void MYelease(GameObject ui);
	public abstract void MYoubleClick(GameObject ui);
	public abstract void MYonInput(GameObject ui, string c);
	public abstract void MYondrag(Vector2 delta);
}
