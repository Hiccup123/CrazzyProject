using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class MainCityUI : MonoBehaviour {

    private GComponent m_mainPanel;
    private ShowerUI m_shower_ui;

    public static Dictionary<int, string> m_mainBtnDic = new Dictionary<int, string>()
    {
        { 0,"neizi"},{ 1,"shiwu"},{ 2,"xizao"},{ 3,"chanzi"},{ 4,"fumo"},{ 5,"beibao"},
    };

    public List<GObject> m_animal_list = new List<GObject>();

    void Awake()
    {
        GameManager.Awake();
    }

    void Start()
    {
        m_mainPanel = this.GetComponent<UIPanel>().ui;

        //m_mainPanel.SetSize(640, 960);
        //Debug.Log("m_mainPanel.width:" + m_mainPanel.width);
        //Debug.Log("m_mainPanel.height:" + m_mainPanel.height);
        //Debug.Log("m_mainPanel:" + m_mainPanel.x + "||" + m_mainPanel.y);
        //m_mainPanel.y = (GRoot.inst.height - m_mainPanel.height) / 2;

        GRoot.inst.SetContentScaleFactor(640, 960, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
        //Debug.Log(GRoot.inst.height);
        //Debug.Log(m_mainPanel.height);
        //m_mainPanel.y = (GRoot.inst.height - m_mainPanel.height) / 2;

        m_shower_ui = m_mainPanel.GetChild("AnimalPlace").asCom as ShowerUI;

        GList m_mainBtnList = m_mainPanel.GetChild("MainBtnList").asList;
        m_mainBtnList.RemoveChildrenToPool();
        for (int i = 0; i < 6; i++)
        {
            GButton p_g_ob = m_mainBtnList.AddItemFromPool ().asButton;
            p_g_ob.name = "B_" + i;
            p_g_ob.icon = UIPackage.GetItemURL("MainPack", "hpg_btn_" + m_mainBtnDic[i]);
            p_g_ob.onClick.Add(MainBtnEventCallBack);
        }
        m_mainPanel.GetChild("LT_0").onClick.Add(MainBtnEventCallBack);
        m_mainPanel.GetChild("TR_0").onClick.Add(MainBtnEventCallBack);
        m_mainPanel.GetChild("TR_1").onClick.Add(MainBtnEventCallBack);

        GComponent m_animalPlace = m_mainPanel.GetChild("AnimalPlace").asCom;

        Rect t_rect = m_animalPlace.TransformRect(new Rect(0, -100, m_animalPlace.width, m_animalPlace.height + 100), GRoot.inst);
        m_shower_ui.InItShowerUI(t_rect);

        //fight_btn
        GButton t_fight_btn = m_mainPanel.GetChild("n22").asButton;
        t_fight_btn.onClick.Add(FightBegin);
    }

    void MainBtnEventCallBack(EventContext p_event_context)
    {
        string t_btn_name = ((GObject)(p_event_context.sender)).name;
        Debug.Log("t_btn_name:" + t_btn_name);
        if (t_btn_name.Substring (0,3) == "LT_")
        {
            int t_btn_index = int.Parse(t_btn_name.Substring (3));
            Debug.Log("t_btn_index:" + t_btn_index);
            switch (t_btn_index)
            {
                case 0:
                    Debug.Log("菜单按钮");
                    Debug.Log("MenuUI:" + GameManager.CheckWindow ("MenuUI"));

                    GameManager.OpenWindow("MenuUI", new MenuUI());

                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
        else if (t_btn_name.Substring(0, 3) == "TR_")
        {
            int t_btn_index = int.Parse(t_btn_name.Substring(3));
            Debug.Log("t_btn_index:" + t_btn_index);
            switch (t_btn_index)
            {
                case 0:
                    Debug.Log("宝石");
                    break;
                case 1:
                    Debug.Log("金币");
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
        else if (t_btn_name.Substring(0, 2) == "B_")
        {
            int t_btn_index = int.Parse(t_btn_name.Substring(2));
            Debug.Log("t_btn_index:" + t_btn_index);
            switch (t_btn_index)
            {
                case 0:
                    Debug.Log("JiaZi");
                    GameManager.ShowWindow();
                    break;
                case 1:
                    Debug.Log("Food");
                    GameManager.OpenWindow("FoodUI", new FoodUI());

                    break;
                case 2:
                    Debug.Log("Shower");
                    GameManager.ShowWindow();
                    break;
                case 3:
                    Debug.Log("ChanZi");
                    GameManager.ShowWindow();
                    break;
                case 4:
                    Debug.Log("Hand");
                    GameManager.ShowWindow();
                    break;
                case 5:
                    Debug.Log("Bag");
                    GameManager.ShowWindow();
                    break;
                default:
                    break;
            }

            ControlState t_cur_state = (ControlState)System.Enum.ToObject(typeof(ControlState), t_btn_index);
            m_shower_ui.SetShowerVisable(t_cur_state == ControlState.Shower);
            if (AnimalManager.m_instance.Control_State != t_cur_state)
            {
                AnimalManager.m_instance.SetState(t_cur_state);
            }
            else
            {
                AnimalManager.m_instance.SetState(ControlState.KongXian);
            }
            AnimalManager.m_instance.SetShitCleanState(t_cur_state == ControlState.ChanShi);
        }
        else if (t_btn_name.Substring(0, 3) == "BL_")
        {
            int t_btn_index = int.Parse(t_btn_name.Substring(3, t_btn_name.Length - 3));
            Debug.Log("t_btn_index:" + t_btn_index);
            switch (t_btn_index)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
    }


    //fight_btn
    void FightBegin()
    {
        Debug.Log("enter fight");
        GameManager.RemoveAllWindow();
        LevelManager.Instance.LoadLevel("FightScene",m_mainPanel.position);
    }
}
