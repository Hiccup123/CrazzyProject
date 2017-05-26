using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YuYingTest : MonoBehaviour {

    public InputField ShuRuBox;

    public void StartYuYing()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("StartActivity");
    }

    //接收语音返回的结果
    public void LiJia(string str)
    {
        ShuRuBox.text = str;
    }
}
