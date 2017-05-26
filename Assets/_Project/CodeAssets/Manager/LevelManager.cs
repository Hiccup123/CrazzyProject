using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;

public class LevelManager : MonoBehaviour {

    private static LevelManager m_instance;

    public static LevelManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                GameObject t_instance = new GameObject("LevelManager");
                DontDestroyOnLoad(t_instance);
                m_instance = t_instance.AddComponent<LevelManager>();
            }

            return m_instance;
        }
    }

    GComponent m_loading_ui;

    public LevelManager ()
    {

    }

    void Awake ()
    {
        m_loading_ui = UIPackage.CreateObject("MainPack", "LoadingUI").asCom;
    }

    public void InIt ()
    {
        m_loading_ui.SetSize(GRoot.inst.width,GRoot.inst.height);
        
        m_loading_ui.AddRelation(GRoot.inst, RelationType.Size);
    }

    public void LoadLevel(string p_name,Vector3 p_pos)
    {
        StartCoroutine(DoLoad(p_name));
        GRoot.inst.AddChild(m_loading_ui);
        m_loading_ui.position = p_pos;
    }

    IEnumerator DoLoad(string p_scene_name)
    {
        GRoot.inst.AddChild(m_loading_ui);
        GProgressBar t_bar = m_loading_ui.GetChild("n0").asProgress;
        t_bar.value = 0;

        AsyncOperation t_op = SceneManager.LoadSceneAsync(p_scene_name);
        float t_start_time = Time.time;
        while(!t_op.isDone || t_bar.value != 100)
        {
            int t_value = (int)((Time.time - t_start_time) * 100f / 3f);
            if(t_value > 100)
            {
                t_value = 100;
            }
            t_bar.value = t_value;
            yield return null;
        }

        GRoot.inst.RemoveChild(m_loading_ui);
    }
}
