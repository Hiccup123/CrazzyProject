using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Fight;

public class GameManager{

    private static Dictionary<string, Window> m_windowDic = new Dictionary<string, Window>();

    public static Window m_top_win;

    public static void Awake()
    {
        UIPackage.AddPackage("_UI/FairyUI/MainPack");
        UIPackage.AddPackage("_UI/FairyUI/MenuPack");
        UIPackage.AddPackage("_UI/FairyUI/ComPack");
        UIPackage.AddPackage("_UI/FairyUI/FoodPack");
        UIPackage.AddPackage("_UI/FairyUI/ShopPack");
        UIPackage.AddPackage("_UI/FairyUI/PetCollectPack");
        UIPackage.AddPackage("_UI/FairyUI/PetMsgPack");
        UIPackage.AddPackage("_UI/FairyUI/FightPack");
        UIPackage.AddPackage("_UI/FairyUI/EffectPack");

        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("PetCollectPack", "PetItem"), typeof(CollectSingle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("FightPack", "FItem"), typeof(FightSingle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("FightPack", "FightMsg"), typeof(FightMsg));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("FightPack", "FPlayerMsg"), typeof(FightPlayerInfo));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("FightPack", "FAnimalSingle"), typeof(FAnimalSingle));
        //UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainPack", "CAnimal"), typeof(CAnimal));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainPack", "CAnimal"), typeof(AIControl));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainPack", "Top"), typeof(CAnimalTop));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainPack", "AnimalPlace"), typeof(ShowerUI)); 
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainPack", "CShit"), typeof(ShitSingle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainPack", "CFood"), typeof(CFoodSingle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("FoodPack", "FoodItem"), typeof(FoodSingle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("PetMsgPack", "Page_JiBen"), typeof(PetMsgJiBen));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("PetMsgPack", "PetMsgItem"), typeof(PetMsgSingle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("EffectPack", "FAttackSingle"), typeof(FightAttackSingle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("FightPack", "SkillGroup"), typeof(FightSkillGroup));

        ActTemplate.LoadTemplate();
        PetBaseTemplate.LoadTemplate();
        SoundListTemplate.LoadTemplate();

        SoundManager.InItManager();

        UIConfig.buttonSound = SoundManager.GetAudioClip(201);
        Application.targetFrameRate = 60;

        Stage.inst.onKeyDown.Add(OnKeyDown);
        LevelManager.Instance.InIt();
        //Debug.Log("GRoot.contentScaleFactor:" + GRoot.contentScaleFactor);
        //GRoot.inst.SetContentScaleFactor(640, 960, UIContentScaler.ScreenMatchMode.MatchWidth);
        //Debug.Log("GRoot.contentScaleFactor1:" + GRoot.contentScaleFactor);
        //Debug.Log("GRoot.width:" + GRoot.inst.width);
        //Debug.Log("GRoot.height:" + GRoot.inst.height);
        //Debug.Log("Screen.width:" + Screen.width);
        //Debug.Log("Screen.height:" + Screen.height);

        FontManager.RegisterFont(FontManager.GetFont("HeiTi"), "黑体");
        //UIConfig.defaultFont = "黑体";
        FontManager.GetFont("黑体").customBold = true;
        //UIConfig.buttonSound = ;
    }

    static void OnKeyDown(EventContext context)
    {
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            Application.Quit();
        }
    }

    #region WindowManager
    /// <summary>
    /// 打开一个window
    /// </summary>
    /// <param name="p_win_name"></param>
    /// <param name="p_win"></param>
    public static void OpenWindow(string p_win_name, Window p_win)
    {
        if (!GameManager.CheckWindow(p_win_name))
        {
            GameManager.AddWindowToDic(p_win, p_win_name);
        }

        GameManager.ShowWindow(p_win_name);
    }

    /// <summary>
    /// 将一个窗口添加到窗口列表
    /// </summary>
    /// <param name="p_win"></param>
    /// <param name="p_name"></param>
    public static void AddWindowToDic(Window p_win,string p_name)
    {
        if (!m_windowDic.ContainsKey (p_name))
        {
            m_windowDic.Add(p_name,p_win);
        }
    }
    
    /// <summary>
    /// 获得想要显示的窗口
    /// </summary>
    /// <param name="p_name"></param>
    /// <returns></returns>
    public static Window GetWindow(string p_name)
    {
        if (!CheckWindow (p_name))
        {
            Debug.LogError(p_name + "is not exit.");
            return null;
        }
        return m_windowDic[p_name];
    }

    /// <summary>
    /// 检查想要显示的窗口是否存在
    /// </summary>
    /// <param name="p_name"></param>
    /// <returns></returns>
    public static bool CheckWindow(string p_name)
    {
        if (m_windowDic.ContainsKey (p_name))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 显示一个窗口或关闭一个窗口
    /// </summary>
    /// <param name="p_win_name"></param>
    public static void ShowWindow(string p_win_name = null)
    {
        if (p_win_name == null)
        {
            GRoot.inst.CloseAllWindows();
            return;
        }
        Debug.Log("topWindow:" + GRoot.inst.GetTopWindow ());
        if (GRoot.inst.GetTopWindow() != null)
        {
            if (GRoot.inst.GetTopWindow() != GameManager.GetWindow(p_win_name))
            {
                GRoot.inst.HideWindow(GRoot.inst.GetTopWindow());
                GameManager.GetWindow(p_win_name).Show();
            }
            else
            {
                GRoot.inst.HideWindow(GRoot.inst.GetTopWindow());
            }
        }
        else
        {
            GameManager.GetWindow(p_win_name).Show();
        }
    }

    /// <summary>
    /// 清空所有窗口
    /// </summary>
    public static void RemoveAllWindow()
    {
        m_windowDic.Clear();
        GRoot.inst.CloseAllWindows();
    }
    #endregion

    #region LoadPrefab
    public static GameObject LoadObj(GGraph p_graph,Vector3 p_graph_pos,string p_path,Vector3 p_load_obj_pos,Vector3 p_scale)
    {
        Object t_source_obj = UnityEngine.Resources.Load(p_path);
        GameObject t_obj = (GameObject)Object.Instantiate(t_source_obj);
        t_obj.transform.localPosition = p_load_obj_pos;
        t_obj.transform.localScale = p_scale;
        t_obj.transform.localEulerAngles = Vector3.zero;
        p_graph.asGraph.SetNativeObject(new GoWrapper(t_obj));
        p_graph.asGraph.position = p_graph_pos;

        return t_obj;
    }
    #endregion
}
