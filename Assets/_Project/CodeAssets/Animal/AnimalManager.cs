using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System.Linq;

public class AnimalManager : AnimalInstance<AnimalManager> {

    private GComponent m_mainPanel;

    private GComponent m_animalPlace;

    private ControlState m_control_state;
    public ControlState Control_State { get { return m_control_state; } }

    private List<GObject> m_animal_list = new List<GObject>();
    private List<GObject> m_food_list = new List<GObject>();
    private List<GObject> m_totle_list = new List<GObject>();

    private List<GObject> m_shit_list = new List<GObject>();
    public int ShitCount { get { return m_shit_list.Count; } }

    private Rect m_rect;

    private List<BaseData> m_data_list = new List<BaseData>();

    private float m_update_time;    //层的刷新时间

    void Start()
    {
        m_mainPanel = this.GetComponent<UIPanel>().ui;

        m_animalPlace = m_mainPanel.GetChild("AnimalPlace").asCom;

        m_rect = m_animalPlace.TransformRect(new Rect(0, 0, m_animalPlace.width, m_animalPlace.height), GRoot.inst);
        
        //CreateAnimal

        for (int i = 0;i < 5;i ++)
        {
            BaseData p_data = new BaseData();
            p_data.m_id = 1011;
            p_data.m_hunger = Random.Range(0,100);
            p_data.m_health = Random.Range(0,100);
            p_data.m_clean = Random.Range(0,100);
            p_data.m_appease = Random.Range(0,100);
            p_data.m_grow = 0;
            m_data_list.Add(p_data);
        }

        for (int i = 0; i < m_data_list.Count; i++)
        {
            AddAnimal();
            //CreateAnimal();
        }

        m_control_state = ControlState.KongXian;
        SetState(m_control_state);
    }

    #region Add Or Remove Animal
    void AddAnimal()
    {
        GObject t_animal = UIPackage.CreateObject("MainPack", "CAnimal");
        t_animal.name = "Animal";
        m_animalPlace.AddChild(t_animal);
        m_animal_list.Add(t_animal);
        m_totle_list.Add(t_animal);
        SetAnimal(t_animal, new Vector3(Random.Range(35, 465), Random.Range(0, 500), 0));      
        AIControl t_c_animal = t_animal as AIControl;
        t_c_animal.InItItem(m_data_list[m_animal_list.Count - 1], m_rect);
    }

    void SetAnimal(GObject p_obj, Vector3 p_pos)
    {
        p_obj.position = p_pos;
        //Debug.Log("p_pos:" + p_pos);
    }
    #endregion

    #region Add Or Remove Food
    public void AddFood(Vector3 p_pos,string p_name)
    {
        GObject t_food = UIPackage.CreateObject("MainPack", "CFood");
        m_animalPlace.AddChild(t_food);
        m_totle_list.Add(t_food);
        m_food_list.Add(t_food);
        t_food.scale = Vector3.one * 0.5f;
        t_food.position = p_pos;
        CFoodSingle t_single = t_food as CFoodSingle;
        t_single.InItFood(p_name);
    }

    public void RemoveFood(GObject p_obj)
    {
        m_totle_list.Remove(p_obj);
        m_food_list.Remove(p_obj);
        //m_food_list.ForEach(item => { if (item == p_obj) { item.visible = false; } });
        p_obj.Dispose();
    }
    #endregion

    #region Add Or Remove Shit
    public void AddShit(Vector3 p_pos)
    {
        GObject t_shit = UIPackage.CreateObject("MainPack", "CShit");
        m_animalPlace.AddChild(t_shit);
        m_shit_list.Add(t_shit);
        t_shit.scale = Vector3.one * 0.5f;
        t_shit.position = p_pos + new Vector3(10,30);
        t_shit.sortingOrder = -1000;
    }
    public void RemoveShit(GObject p_obj)
    {
        m_shit_list.Remove(p_obj);
        p_obj.Dispose();
    }
    #endregion

    //设置当前状态
    public void SetState(ControlState p_state)
    {
        m_control_state = p_state;

        foreach(var t_item in m_animal_list)
        {
            AIControl t_c_animal = t_item as AIControl;
            //CAnimal t_c_animal = t_item as CAnimal;
            t_c_animal.ChangeControlState (p_state);
        }
    }

    //find food target
    public GObject GetFoodTarget(Vector3 p_pos)
    {
        for (int i = 0;i < m_food_list.Count;i ++)
        {
            if (m_food_list[i].position == p_pos)
            {
                return m_food_list[i].visible ? m_food_list[i] : null;
            }
        }

        return null;
    }
    //food pos
    public Vector3 GetFoodPos (Vector3 p_pos)
    {
        float t_dis = 0;
        Vector3 t_pos = Vector3.one * 10000;
        for (int i = 0;i < m_food_list.Count;i ++)
        {
            if (!m_food_list[i].visible)
            {
                continue;
            }
            if (t_dis == 0)
            {
                t_dis = Vector3.Distance(p_pos, m_food_list[i].position);
                t_pos = m_food_list[i].position;
            }
            else
            {
                if (Vector3.Distance(p_pos, m_food_list[i].position) < t_dis)
                {
                    t_dis = Vector3.Distance(p_pos, m_food_list[i].position);
                    t_pos = m_food_list[i].position;
                }
            }
        }
        return t_pos;
    }

    void Update()
    {
        m_update_time += Time.deltaTime;
        if (m_update_time > 0.5f)
        {
            m_update_time = 0;
            m_totle_list.ForEach((e) => 
            {
                e.sortingOrder = (int)e.y;
            });
        }

        //if (m_control_state == ControlState.Food)
        //{
        //    if (string.IsNullOrEmpty (M_Food))  return;

        //    if (Input.GetMouseButtonDown (0))
        //    {
        //        Debug.Log(GRoot.inst.touchTarget.name);
                
        //        if (GRoot.inst.touchTarget.name != "MainBg")
        //        {
        //            return;
        //        }
        //        m_min_dis = 0;
        //        Vector3 t_pos = Input.mousePosition;
        //        t_pos.y = 960 - t_pos.y -120;
        //        //Debug.Log("t_pos:" + t_pos);
        //        if (t_pos.x > m_rect.xMax || t_pos.x < m_rect.xMin || t_pos.y > 550 || t_pos.y < 50)
        //        {
        //            return;
        //        }
        //        AddFood(t_pos, M_Food);
        //    }
        //}
    }

    //find shower target
    public void FindShowerTarget (GObject p_obj)
    {
        for (int i = 0;i < m_animal_list.Count;i ++)
        {
            float t_dis_x = Mathf.Abs(p_obj.x - m_animal_list[i].x);
            float t_dis_y = Mathf.Abs(p_obj.y - m_animal_list[i].y);
            //Debug.Log(p_obj.y);
            if (t_dis_x <= 70 && t_dis_y <= 190)
            {
                AIControl t_ai = m_animal_list[i] as AIControl;
                t_ai.TakeAShower();
                break;
            }
        }
    }

    //set shit clean state
    public void SetShitCleanState (bool p_value)
    {
        foreach (var item in m_shit_list)
        {
            ShitSingle t_single = item as ShitSingle;
            t_single.SetShitTouchAble(p_value);
        }
    }
}
