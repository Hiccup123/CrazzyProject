using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUIManager : MonoBehaviour {

	public void BtnOnClick(string p_str)
    {
        if(p_str == "bb")
        {
            Debug.Log("Click");
        }
    }
}
