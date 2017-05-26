using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

public class AIControl : GButton {

    private GGraph m_graph;
    private CAnimalTop m_top;
    private AudioSource m_audio;

    private BaseData m_base_data;
    private SpineData m_spine_data;
    private GameObject m_animal_obj;

    private Dictionary<AIAction, AIBase> m_action_dic = new Dictionary<AIAction, AIBase>();

    private AIBase m_cur_state;
    private AIBase m_last_state;
    private ControlState m_control_state;

    //food
    private GObject m_target;
    private Vector3 m_target_pos;
    //food
    private GObject m_food_target;      //目标食物
    private float m_cur_find_time;      //寻找食物间隔时间
    private Vector3 m_food_pos;
    private Vector3 m_null_pos = Vector3.one * 10000;


    //空闲状态下
    private float m_think_time;     //思考时间
    private float m_pass_time;      //移动已经消耗的时间
    private int m_think_index;      //下一次行动代号
    private List<AIAction> m_action_list = new List<AIAction>();

    public override void ConstructFromXML(XML cxml)
    {
        base.ConstructFromXML(cxml);

        m_graph = this.GetChild("Animal").asGraph;
        m_top = this.GetChild("Top").asCom as CAnimalTop;
        GGraph t_graph = this.GetChild("Audio").asCom.GetChild("Audio").asGraph;
        m_audio = SoundManager.SetAudioSource(t_graph);
        
        this.sound = null;
    }

    //初始化item
    public void InItItem(BaseData p_data, Rect p_rect)
    {
        this.dragBounds = p_rect;
        m_top.InItComponent(p_data);
        m_base_data = p_data;

        CreateAnimal(m_base_data);
    }

    //创建animal
    void CreateAnimal(BaseData p_data)
    {
        Object m_animal_prefab = UnityEngine.Resources.Load("_UI/SpineSkeletonData/Animal");
        m_animal_obj = (GameObject)Object.Instantiate(m_animal_prefab);
        m_animal_obj.transform.localPosition = Vector3.zero;
        m_animal_obj.transform.localScale = Vector3.one * 35;
        m_animal_obj.transform.localEulerAngles = Vector3.zero;
        m_graph.asGraph.SetNativeObject(new GoWrapper(m_animal_obj));
        m_graph.asGraph.position = new Vector3(10, 75, 0); //new Vector3(70,100,0)
        m_spine_data = m_animal_obj.GetComponent<SpineData>();
        m_spine_data.SetId(p_data.m_id);

        AddAction();

        m_action_list.Add(AIAction.Idle_Free);
        m_action_list.Add(AIAction.Walk);
        m_action_list.Add(AIAction.Sleep);
        m_action_list.Add(AIAction.Beg_Food);
        m_action_list.Add(AIAction.Beg_Shower);
        m_action_list.Add(AIAction.Beg_Touch);
        m_action_list.Add(AIAction.Shit);

        ChangeAction(AIAction.Idle);
    }

    void AddAction ()
    {
        AIIdleFree t_idle = new AIIdleFree(m_spine_data); 
        AIWalk t_walk = new AIWalk(m_spine_data);
        AISleep t_sleep = new AISleep(m_spine_data);
        AIShit t_shit = new AIShit(m_spine_data);
        AIEat t_eat = new AIEat(m_spine_data);
        AIBegFood t_beg_food = new AIBegFood(m_spine_data);
        AIBegTouch t_beg_touch = new AIBegTouch(m_spine_data);
        AIBegShower t_beg_shower = new AIBegShower(m_spine_data);
        AIBeCaught t_be_caught = new AIBeCaught(m_spine_data);
        AIBeCaughtDis t_be_caught_dis = new AIBeCaughtDis(m_spine_data);
        AIBeTouched t_be_touched = new AIBeTouched(m_spine_data);
        AIBeBathed t_be_bathed = new AIBeBathed(m_spine_data);
        AIBeatOtherAnimals t_beat_other_animals = new AIBeatOtherAnimals(m_spine_data);
        AIBeBeatByOtherAnimals t_be_beat_other_animals = new AIBeBeatByOtherAnimals(m_spine_data);
        AIMedicine t_medchine = new AIMedicine(m_spine_data);
        AIWrongMedicine t_wrong_medchine = new AIWrongMedicine(m_spine_data);

        m_action_dic.Add(AIAction.Idle_Free, t_idle);
        m_action_dic.Add(AIAction.Walk, t_walk);
        m_action_dic.Add(AIAction.Sleep, t_sleep);
        m_action_dic.Add(AIAction.Shit, t_shit);
        m_action_dic.Add(AIAction.Eat, t_eat);
        m_action_dic.Add(AIAction.Beg_Food, t_beg_food);
        m_action_dic.Add(AIAction.Beg_Touch, t_beg_touch);
        m_action_dic.Add(AIAction.Beg_Shower, t_beg_shower);
        m_action_dic.Add(AIAction.Be_Caught, t_be_caught);
        m_action_dic.Add(AIAction.Be_Caught_Dis, t_be_caught_dis);
        m_action_dic.Add(AIAction.Be_Touched, t_be_touched);
        m_action_dic.Add(AIAction.Be_Bathed, t_be_bathed);
        m_action_dic.Add(AIAction.Beat_Other_Animals, t_beat_other_animals);
        m_action_dic.Add(AIAction.Be_Beat_By_Other_Animals, t_be_beat_other_animals);
        m_action_dic.Add(AIAction.Medicine, t_medchine);
        m_action_dic.Add(AIAction.Eat_Wrong_Medicine, t_wrong_medchine);

        this.onTouchBegin.Add(TouchBegin);
        this.onTouchEnd.Add(TouchEnd);
        onClick.Add(Click);
        onDrop.Add(Drap);
    }

    //切换操作状态
    public void ChangeControlState (ControlState p_state)
    {
        m_control_state = p_state;

        this.draggable = p_state == ControlState.Zhua;

        m_top.SetComponent(p_state);

        if (p_state == ControlState.Food || p_state == ControlState.FuMo || p_state == ControlState.Shower || p_state == ControlState.ZhiLiao)
        {
            m_top.SetQiPao(false); 
        }
        switch (p_state)
        {
            case ControlState.Food:
                ChangeAction(AIAction.Beg_Food);
                break;
            case ControlState.FuMo:
                ChangeAction(AIAction.Beg_Touch);
                break;
            case ControlState.Shower:
                ChangeAction(AIAction.Beg_Shower);
                break;
        }
    }

    //切换动作
    void ChangeAction (AIAction p_action)
    {
        m_cur_state = m_action_dic.ContainsKey (p_action) ? m_action_dic[p_action] : null;
        //Debug.Log("m_cur_state:" + m_cur_state);
        if (m_cur_state != null)
        {
            if (m_cur_state != m_last_state)
            {
                m_last_state = m_cur_state;
                AIContext p_context = new AIContext();
                if (p_action == AIAction.Walk)
                {
                    p_context.m_root = this;
                    p_context.m_root_pos = this.position;
                    p_context.m_target = m_target;
                    p_context.m_target_pos = m_target_pos;
                }
                m_cur_state.EnterState(p_context);

                switch (p_action)
                {
                    case AIAction.Eat:
                        SoundManager.Play(301, m_audio);
                        break;
                    case AIAction.Be_Touched:
                        SoundManager.Play(305, m_audio);
                        break;
                    case AIAction.Medicine:
                        SoundManager.Play(303, m_audio);
                        break;
                    case AIAction.Be_Bathed:
                        SoundManager.Play(304, m_audio);
                        break;
                }
            }
        }
        else
        {
            //开始随机活动
            m_pass_time = 0;
            m_last_state = null;
        }
    }

    protected override void OnUpdate()
    {
        if(m_cur_state == null)
        {
            RandomAction();
            return;
        }
        
        if (m_cur_state.OnUpdate () == AIStatus.Fail)
        {
            return;
        }

        if (m_cur_state.OnUpdate () == AIStatus.Success)
        {
            m_cur_state.ExitState();
            ChangeAction(AIAction.Idle);
        }
    }

    #region TouchEvent
    void TouchBegin ()
    {
        if (m_control_state == ControlState.Zhua)
        {
            ChangeAction(AIAction.Be_Caught);
            m_top.SetHand(true);
            m_top.SetQiPao(false);
            m_top.SetHandPos(!m_spine_data.SkeletonAnim().skeleton.FlipX);
        }
    }

    void TouchEnd ()
    {
        if (m_control_state == ControlState.Zhua)
        {
            m_top.SetHand(false);
            if (m_cur_state == m_action_dic[AIAction.Be_Caught])
            {
                m_cur_state.OtherEvent();
                ChangeAction(AIAction.Be_Caught_Dis);
            }
        }
    }
    #endregion

    #region ClickEvent
    void Click ()
    {
        switch (m_control_state)
        {
            //case ControlState.Food:
            //    if (string.IsNullOrEmpty(AnimalManager.m_instance.M_Food))
            //    {
            //        return;
            //    }
            //    ChangeAction(AIAction.Eat);
            //    break;
            case ControlState.FuMo:
                ChangeAction(AIAction.Be_Touched);
                break;
            case ControlState.ZhiLiao:
                ChangeAction(AIAction.Medicine);//ChangeAction(AIAction.Eat_Wrong_Medicine);
                break;
        }
    }
    #endregion

    #region DrapEvent
    void Drap(EventContext p_context)
    {
        Debug.Log("p_context:" + p_context.sender);
        if(m_control_state == ControlState.Food)
        {
            ChangeAction(AIAction.Eat);
        }
    }
    #endregion

    #region RandomAction
    void RandomAction ()
    {
        m_pass_time += Time.deltaTime;
        if (m_pass_time >= m_think_time)
        {
            m_pass_time = 0;
            m_think_time = Random.Range(2, 5);
            if (m_cur_state == null)
            {
                m_think_index = Random.Range(0, m_action_list.Count);
                //Debug.Log("m_think_index:" + m_action_list[m_think_index]);

                bool t_show = m_control_state == ControlState.Food || m_control_state == ControlState.FuMo || 
                    m_control_state == ControlState.Shower || m_control_state == ControlState.ZhiLiao;

                switch (m_action_list[m_think_index])
                {
                    case AIAction.Walk:         //walk random
                        m_target_pos = RandomPos();
                        break;
                    case AIAction.Sleep:        //sleep
                        m_top.SetQiPao(false);
                        break;
                    case AIAction.Beg_Food:     //beg food
                        m_top.SetQiPao(!t_show, 0);
                        break;
                    case AIAction.Beg_Touch:    //beg touch
                        m_top.SetQiPao(!t_show, 1);
                        break;
                    case AIAction.Beg_Shower:   //beg shower
                        m_top.SetQiPao(!t_show, 2);
                        break;
                    case AIAction.Shit:         //shit
                        if (AnimalManager.m_instance.ShitCount < 5)
                        {
                            m_top.SetQiPao(false);
                            AnimalManager.m_instance.AddShit(this.position);
                        }
                        else
                        {
                            return;
                        }
                        break;
                }
                ChangeAction(m_action_list[m_think_index]);
            }
        }
    }
    #endregion

    //随机活动位置
    Vector3 RandomPos()
    {
        float t_x = Random.Range(-100, 100);
        float t_y = Random.Range(-100, 100);

        if (this.position.x + t_x < 0)
        {
            t_x = 35 - this.position.x;
        }
        if (this.position.x + t_x > 500)
        {
            t_x = 465 - this.position.x;
        }
        if (this.position.y + t_y < 0)
        {
            t_y = -this.position.y;
        }
        if (this.position.y + t_y > 530)
        {
            t_y = 530 - this.position.y;
        }
        //Debug.Log("x:" + m_root_pos.Value.x + t_x);
        //Debug.Log("y:" + m_root_pos.Value.y + t_y);
        return new Vector3(this.position.x + t_x, this.position.y + t_y, 0);
    }

    #region FindFood
    void FindFood ()
    {
        if (m_base_data.m_hunger < 100)
        {
            if (m_food_pos == m_null_pos)
            {
                m_cur_find_time += Time.deltaTime;
                if (m_cur_find_time > 2)
                {
                    m_cur_find_time = 0;
                    if (AnimalManager.m_instance.GetFoodPos(this.position) != m_null_pos)
                    {
                        m_food_pos = AnimalManager.m_instance.GetFoodPos(this.position);

                        ChangeAction(AIAction.Walk);
                    }
                }
            }
        }
    }
    #endregion

    #region Take a shower
    public void TakeAShower ()
    {
        ChangeAction(AIAction.Be_Bathed);
    }
    #endregion
}
