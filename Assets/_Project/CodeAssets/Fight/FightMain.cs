using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;
using Spine.Unity;
using Fight;

public delegate void Attack(bool p_value = false);  //攻击
public delegate void FightEvent();

public delegate void EventDelegate();
public delegate void EventDelegate1(GObject obj);

public class FightMain : MonoBehaviour
{
    public CameraShake m_camera_shake;

    private GComponent m_fight_panel;
    private GComponent m_item_group;
    private GComponent m_arrow_group;
    private GComponent m_skill_group;

    private FightSkillGroup m_fight_skill_group;
    private FightMsg m_fight_msg;

    public static GObject S_Hover_Obj;     //鼠标或手指当前停留的obj
    public GObject Temp_Obj; //临时obj,用于下一个HoverObj的检测
    public GObject Cur_Obj;  //用于存储当前点击的图片

    public GObject[,] Fight_List;   //存储所有图片数组
    public List<GObject> Dispose_List = new List<GObject>();   //用于存储将被消除的图片
    public List<GObject> Arrow_List = new List<GObject>();    //用于存储箭头
    public List<List<GObject>> Comb_List = new List<List<GObject>>();   //可消除组合
    public List<GObject> AlphaList = new List<GObject>();   //变透明的list
    private GObjectPool m_arrow_pool;   //箭头对象池

    public int Min_Dis_Count = 3;    //最低消除的个数
    private int m_dis = 10;      //方格之间距离

    public float Speed = 0.1f;   //下落速度
    private float m_panel_w;    //界面宽度
    private float m_panel_h;    //界面高度
    private float m_item_w = 70; //图片宽
    private float m_item_h = 70; //图片高
    private float m_border_x;   //x轴边距
    private float m_border_y;   //y轴边距

    private Dictionary<FightAction, FightBase> m_fight_dic = new Dictionary<FightAction, FightBase>();
    private FightAction m_cur_action;   //当前状态
    private FightBase m_cur_status;     //当前执行

    //玩家信息
    private static PlayerType m_cur_type;  //当前行动玩家
    private int m_step_count = 3;   //最大行动步数
    private static int m_cur_step_count;   //当前行动步数
    private float m_enemy_think_time = 0;   //敌人思考时间
    private bool m_enemy_can_think = false; //敌人是否开始思考

    private static bool m_attacking;       //是否在攻击
    private static bool m_showing_skill;   //是否在播放技能

    private Dictionary<string, AudioSource> m_audio_dic = new Dictionary<string, AudioSource>();
    private string m_tip_audio = "TipAudio";
    private string m_remove_audio = "RemoveAudio";

    void Awake()
    {
        GameManager.Awake();
    }

    void Start()
    {
        m_fight_panel = this.GetComponent<UIPanel>().ui;
        //m_fight_panel.SetSize(640, 960);
        GRoot.inst.SetContentScaleFactor(640, 960, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
        //m_fight_panel.y = (GRoot.inst.height - m_fight_panel.height) / 2;

        m_item_group = m_fight_panel.GetChild("ItemGroup").asCom;
        m_skill_group = m_fight_panel.GetChild("SkillGroup").asCom;
        m_arrow_group = m_fight_panel.GetChild("ArrowGroup").asCom;

        m_arrow_pool = new FairyGUI.GObjectPool(m_arrow_group.displayObject.cachedTransform);

        m_fight_skill_group = m_skill_group as FightSkillGroup;

        InItPlayerData();
        CreateItems();

        //注册各种状态
        FightBase t_line = new FightLine(this);
        m_fight_dic.Add(FightAction.Line, t_line);
        FightBase t_cancal_line = new FightCancelLine(this);
        m_fight_dic.Add(FightAction.Cancel_Line, t_cancal_line);
        FightBase t_remove = new FightRemove(this);
        m_fight_dic.Add(FightAction.Remove, t_remove);
        FightBase t_rebuild = new FightRebuild(this);
        m_fight_dic.Add(FightAction.Rebuild, t_rebuild);
        FightBase t_repair = new FightRepair(this);
        m_fight_dic.Add(FightAction.Repair, t_repair);
        FightBase t_check_comb = new FightCheckComb(this);
        m_fight_dic.Add(FightAction.Check_Comb, t_check_comb);

        m_fight_panel.GetChild("Btn_Quit_Fight").asButton.onClick.Add(() =>
        {
            LevelManager.Instance.LoadLevel("MainScene", m_fight_panel.position);
        });

        {
            m_cur_step_count = 0;
            m_attacking = false;
            m_showing_skill = false;
        }

        m_cur_type = PlayerType.P_Self;
        ChangeTurn();
        SetState();
        ChangeStatus(FightAction.Check_Comb);
    }

    //创建items
    void CreateItems()
    {
        m_panel_w = m_item_group.width;
        m_panel_h = m_item_group.height;

        //Debug.Log("panel_w:" + m_panel_w);
        //Debug.Log("panel_h:" + m_panel_h);
        Fight_List = new GObject[8, 7];

        m_border_x = (m_panel_w - (m_item_w + m_dis) * Fight_List.GetLength(1) - m_dis) / 2;
        m_border_y = (m_panel_h - (m_item_h + m_dis) * Fight_List.GetLength(0) - m_dis) / 2;
        //Debug.Log("m_border_x:" + m_border_x);
        //Debug.Log("m_border_y:" + m_border_y);

        int t_num = 0;
        for (int i = 0; i < Fight_List.GetLength(0); i++)
        {
            for (int j = 0; j < Fight_List.GetLength(1); j++)
            {
                //消除的方块
                var t_temp = UIPackage.CreateObject("FightPack", "FItem");
                Fight_List[i, j] = t_temp;
                m_item_group.AddChild(t_temp);
                t_temp.scale = Vector3.one;
                t_temp.position = BoxPos(i, j);
                Fight_List[i, j].name = "FItem" + t_num;
                Fight_List[i, j].gameObjectName = "FItem" + t_num;
                //Debug.Log("m_fight_items[i, j].name:" + m_fight_items[i, j].name); 

                FightSingle t_f_single = Fight_List[i, j] as FightSingle;
                t_f_single.Box_Index = new BoxIndex(i, j);  //设置下标
                if (i == Fight_List.GetLength(0) - 1)
                {
                    if (j % 2 != 0)
                    {
                        t_f_single.SetIcon(0);
                    }
                    else
                    {
                        t_f_single.SetIcon(GetRandomId());
                    }
                }
                else
                {
                    t_f_single.SetIcon(GetRandomId());
                }
                t_f_single.Even = j % 2 != 0;
                t_f_single.Choose = false;

                t_num++;
            }
        }
    }

    //初始化玩家数据
    void InItPlayerData()
    {
        int[] t_list = new int[] { 1001, 1011, 1021, 1031 };

        FightData t_self_data = new FightData();
        t_self_data.m_blood = 5000;
        t_self_data.m_name = "Hiccup";
        for (int i = 0; i < t_list.Length; i++)
        {
            FAnimalData t_data = new FAnimalData();

            PetBaseTemplate t_template = PetBaseTemplate.GetPetBaseTemplateById(t_list[i]);
            t_data.m_animal_type = (FAnimalData.FAnimalType)Enum.ToObject(typeof(FAnimalData.FAnimalType), t_template.TYPEID);
            t_data.m_power_value = 0;
            t_data.m_power_max_value = 100;
            t_self_data.m_data_list.Add(t_data);
        }

        FightData t_enemy_data = new FightData();
        t_enemy_data.m_blood = 5000;
        t_enemy_data.m_name = "Boss";
        for (int i = 0; i < t_list.Length; i++)
        {
            FAnimalData t_data = new FAnimalData();
            PetBaseTemplate t_template = PetBaseTemplate.GetPetBaseTemplateById(t_list[i]);
            t_data.m_animal_type = (FAnimalData.FAnimalType)Enum.ToObject(typeof(FAnimalData.FAnimalType), t_template.TYPEID);
            t_data.m_power_value = 0;
            t_data.m_power_max_value = 100;
            t_enemy_data.m_data_list.Add(t_data);
        }

        m_fight_msg = m_fight_panel.GetChild("UI").asCom as FightMsg;
        m_fight_msg.SetPlayerInfo(PlayerType.P_Self, t_self_data,() => 
        {
            m_showing_skill = true;
            m_fight_skill_group.SkillShow(PlayerType.P_Self,() =>
            {
                //减血
                m_fight_msg.UpdateBlood(m_cur_type == PlayerType.P_Enemy ? PlayerType.P_Self : PlayerType.P_Enemy, -150);
                Shake();
            }, () => 
            {
                Debug.Log("End");
                m_showing_skill = false;
            });
        });
        m_fight_msg.SetPlayerInfo(PlayerType.P_Enemy, t_enemy_data,() =>
        {
            m_showing_skill = true;
            m_fight_skill_group.SkillShow(PlayerType.P_Enemy, () =>
            {
                //减血
                m_fight_msg.UpdateBlood(m_cur_type == PlayerType.P_Enemy ? PlayerType.P_Self : PlayerType.P_Enemy, -150);
                Shake();
            }, () => 
            {
                m_showing_skill = false;
            });
        });
    }

    //切换当前执行状态
    public void ChangeStatus(FightAction p_action)
    {
        //Debug.Log("p_action:" + p_action);
        m_cur_action = p_action;
        //Debug.Log("m_fight_dic[p_action]:" + m_fight_dic[p_action]);
        m_fight_dic[p_action].OnExcute();
        m_cur_status = m_fight_dic[p_action];
        //m_attacking = p_action != FightAction.Line;
    }

    void Update()
    {
        if (m_cur_action == FightAction.Line)
        {
            if(!m_showing_skill)
            {
                if (!m_attacking)
                {
                    if (m_cur_type == PlayerType.P_Enemy)
                    {
                        if (m_enemy_can_think)
                        {
                            m_enemy_think_time += Time.deltaTime;
                            if (m_enemy_think_time > 1)
                            {
                                m_enemy_think_time = 0;
                                if (m_fight_msg.CheckAutoSkill(m_cur_type))
                                {
                                    m_fight_msg.AutoSkill(m_cur_type);
                                }
                                else
                                {
                                    m_enemy_can_think = false;
                                    EnemyAction();
                                }
                            }
                        }
                    }
                    else
                    {
                        m_cur_status.InputUpdate();
                    }
                }
            }
        }

        if (m_cur_status.OnUpdate() == FightStatus.Finish)
        {
            ChangeStatus(m_cur_status.TurnNextAction());
        }
    }

    #region IsFriend
    //挨着的是否可以消除
    public bool IsFriend(FightSingle p_hover, FightSingle p_temp)
    {
        //相邻的item在x轴与y轴的行列数不超过1
        int t_x1 = p_hover.Box_Index.m_index_row;
        int t_y1 = p_hover.Box_Index.m_index_col;
        int t_x2 = p_temp.Box_Index.m_index_row;
        int t_y2 = p_temp.Box_Index.m_index_col;

        if (Mathf.Abs(t_x1 - t_x2) <= 1 && Mathf.Abs(t_y1 - t_y2) <= 1)
        {
            if (p_hover.Even)
            {
                if (p_temp.Even)
                {
                    return true;
                }
                else
                {
                    return t_x1 <= t_x2;
                }
            }
            else
            {
                if (p_temp.Even)
                {
                    return t_x1 >= t_x2;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region Remove
    /// <summary>
    /// 移除item
    /// </summary>
    /// <param name="p_recreate"></param>//is rebuild
    /// <param name="p_event"></param>
    public void RemoveItem(bool p_recreate, EventDelegate p_end_event)
    {
        m_cur_step_count++;
        SetState();

        //m_fight_skill_group.SkillShow(PlayerType.P_Self, () =>
        //{
        //    Shake();
        //},null);

        bool t_five = Dispose_List.Count >= 5;  //是否消除5个或以上
        
        FightSingle t_cur_single = Dispose_List[0] as FightSingle;
        bool t_contain_type = t_cur_single.Type > 0 && t_cur_single.Type < 5;   //是否释放火球攻击
        
        if(t_contain_type)
        {
            m_attacking = true;
        }

        //m_fight_skill_group.SkillShow(m_cur_type,null);
        CleanArrow();
        int t_remove_count = Dispose_List.Count;
        int t_cur_remove_count = 0;
        Tweener t_tweener;
        for (int i = 0; i < Dispose_List.Count; i++)
        {
            FightSingle t_single = Dispose_List[i] as FightSingle;
            t_single.Choose = true;

            if (!p_recreate)
            {
                //消除特效
                t_single.PlaySet(t_five, () =>
                {
                    t_tweener = t_single.GetIcon.TweenScale(Vector3.zero, Speed * 2f);
                },() =>
                {
                    //Debug.Log("p_end");
                    if(t_contain_type)
                    {
                        AttackEffect(t_single.position, t_five,() =>
                        {
                            m_fight_msg.UpdateBlood(m_cur_type == PlayerType.P_Enemy ? PlayerType.P_Self : PlayerType.P_Enemy, -50);
                            t_cur_remove_count++;
                            if (t_cur_remove_count >= t_remove_count)
                            {
                                CheckEndState();
                            }
                        });
                    }
                    else
                    {
                        t_cur_remove_count++;
                        if (t_cur_remove_count >= t_remove_count)
                        {
                            CheckEndState();
                        }
                    }
                    m_fight_msg.UpdatePowerBar(m_cur_type, t_single.Type, 10);//更新能量条
                    
                    p_end_event();
                });
            }
            else
            {
                t_tweener = t_single.GetIcon.TweenScale(Vector3.zero, Speed * 2f);
                t_tweener.OnComplete(() =>
                {
                    p_end_event();
                });
            }
        }
    }

    //火球攻击特效
    void AttackEffect(Vector3 p_pos,bool m_five,EventDelegate p_attack_end)
    {
        m_fight_skill_group.GetEffectFromPool(m_cur_type, p_pos, () => 
        {
            Shake();
            p_attack_end();
        }, m_five);
    }

    void Shake()
    {
        m_camera_shake.SetCamera();
    }
    #endregion

    #region Arrow
    public void AddArrow()
    {
        int t_index = Dispose_List.Count - 1;

        if (t_index > 0)
        {
            //add 一个箭头
            GObject t_arrow = m_arrow_pool.GetObject(UIPackage.GetItemURL("FightPack", "Link"));
            //GObject t_arrow = UIPackage.CreateObject("FightPack", "Link");
            t_arrow.visible = true;
            m_arrow_group.AddChild(t_arrow);
            t_arrow.scale = Vector3.one;
            t_arrow.position = Dispose_List[t_index].position - (Dispose_List[t_index].position - Dispose_List[t_index - 1].position) / 2;  

            int t_x1 = 0;
            int t_y1 = 0;
            int t_x2 = 0;
            int t_y2 = 0;

            FightSingle t_single1 = Dispose_List[t_index] as FightSingle;
            FightSingle t_single2 = Dispose_List[t_index - 1] as FightSingle;

            

            RotateAngle(t_arrow, t_x1 - t_x2,t_y1 - t_y2);
            t_arrow.name = "Link" + t_index;
            Arrow_List.Add(t_arrow);
        }
    }

    public void RemoveArrow()
    {
        var t_item = Arrow_List[Arrow_List.Count - 1];
        t_item.visible = false;
        m_arrow_pool.ReturnObject(t_item);
        Arrow_List.Remove(t_item);
    }

    public void CleanArrow()
    {
        for (int i = Arrow_List.Count - 1; i >= 0; i--)
        {
            Arrow_List[i].visible = false;
            m_arrow_pool.ReturnObject(Arrow_List[i]);
        }
        Arrow_List.Clear();
    }

    //箭头旋转角度
    private void RotateAngle(GObject p_arrow,int t_temp1, int t_temp2)
    {
        Debug.Log("temp1:" + t_temp1 + "  And  " + "temp2:" + t_temp2);
        float t_x = 0;
        float t_y = 0;
        float t_angle = 30;

        if (t_temp1 == 0 && t_temp2 == 1)// + new Vector3(52,0)*
        {
            
        }

        if (t_temp1 == 0 && t_temp2 == -1)// + new Vector3(52,0)*
        {
            
        }

        if (t_temp1 == 1 && t_temp2 == 0)// + new Vector3(80,80)*
        {
            
        }

        if (t_temp1 == 1 && t_temp2 == 1)//(87,23)*
        {
            
        }
      
        if (t_temp1 == -1 && t_temp2 == 1)
        {
            
        }

        if (t_temp1 == -1 && t_temp2 == 0)//(-3,40)*
        {
            
        }

        if (t_temp1 == -1 && t_temp2 == -1)//（77，33)*
        {
            
        }
        
        if (t_temp1 == 1 && t_temp2 == -1)//(70,75)*
        {

        }
        p_arrow.rotation = t_angle;
        p_arrow.position += new Vector3(t_x,t_y);
    }
    #endregion

    //图片随机id
    public int GetRandomId(int p_id = -1)
    {
        return p_id == -1 ? UnityEngine.Random.Range(1, 7) : p_id;
    }

    //格子坐标
    private Vector3 BoxPos(int p_i, int p_j)
    {
        float t_x = m_border_x + (m_item_w + m_dis) * p_j + m_dis;
        float t_y = m_border_y + (m_item_h + m_dis) * p_i + m_dis + (p_j % 2 == 0 ? 0 : (m_item_h + m_dis) / 2);

        return new Vector3(t_x, t_y, 0);
    }

    #region PlayerControl
    //设置行动状态
    private void SetState()
    {
        switch (m_cur_type)
        {
            case PlayerType.P_Self:
                m_fight_msg.UpdateStateText(PlayerType.P_Self, m_step_count - m_cur_step_count);
                m_fight_msg.UpdateStateText(PlayerType.P_Enemy, -1);
                break;
            case PlayerType.P_Enemy:
                m_fight_msg.UpdateStateText(PlayerType.P_Self, -1);
                m_fight_msg.UpdateStateText(PlayerType.P_Enemy, m_step_count - m_cur_step_count);
                break;
        }
    }

    //切换回合
    void ChangeTurn()
    {
        m_fight_msg.ChangeTurn(m_cur_type == PlayerType.P_Enemy, () => { m_enemy_can_think = true; });
    }

    //切换角色
    void ChangeController()
    {
        if (m_cur_type == PlayerType.P_Self)
        {
            if (m_cur_step_count >= m_step_count)
            {
                m_enemy_think_time = 0;
                m_cur_step_count = 0;
                m_cur_type = PlayerType.P_Enemy;
                ChangeTurn();
                SetState();
            }
        }
        else
        {
            if (m_cur_step_count >= m_step_count)
            {
                m_cur_step_count = 0;
                m_cur_type = PlayerType.P_Self;
                ChangeTurn();
            }
            else
            {
                m_enemy_think_time = 0;
                m_enemy_can_think = true;
            }
            SetState();
        }
    }

    //敌人行动
    private void EnemyAction()
    {
        int t_ran_comb = UnityEngine.Random.Range(0, Comb_List.Count);
        //Debug.Log("t_ran_comb:" + t_ran_comb);
        for (int i = 0; i < Comb_List[t_ran_comb].Count; i++)
        {
            Dispose_List.Add(Comb_List[t_ran_comb][i]);
        }

        ChangeStatus(FightAction.Remove);
    }
    #endregion

    #region CheckEnd

    //检测死亡/切换行动角色
    void CheckEndState()
    {
        m_attacking = false;
        //Debug.Log("m_animating:" + m_animating);
        if (m_fight_msg.CheckDead(m_cur_type == PlayerType.P_Self ? PlayerType.P_Enemy : PlayerType.P_Self))
        {
            m_fight_msg.ShowEndUI(m_cur_type);
        }
        else
        {
            //判断是否切换行动角色，重置步数 
            ChangeController();
        }
    }

    #endregion

    //声音播放
    void SoundPlay(string p_name, int p_id, bool p_loop = false)
    {
        if (!m_audio_dic.ContainsKey(p_name))
        {
            AudioSource t_source = gameObject.AddComponent<AudioSource>();
            m_audio_dic.Add(p_name, t_source);
        }
        SoundManager.Play(p_id, m_audio_dic[p_name], p_loop);
    }

    //是否可点击释放技能
    public static bool CanClick()
    {
        return !m_attacking && !m_showing_skill && m_cur_type == PlayerType.P_Self && m_cur_step_count <= 3;
    }

    #region GetAlphaList

    public void GetAlphaList(int p_type)
    {
        AlphaList.Clear();
        for (int i = 0; i < Fight_List.GetLength(0);i ++)
        {
            for(int j = 0;j < Fight_List.GetLength(1);j ++)
            {
                FightSingle t_single = Fight_List[i, j] as FightSingle;
                if(t_single.Type != p_type)
                {
                    //t_single.GetIcon.alpha = 0.8f;
                    t_single.GetIcon.color = Color.gray;
                    AlphaList.Add(Fight_List[i,j]);
                }
            }
        }
    }

    public void SetAlphList()
    {
        foreach(var t_obj in AlphaList)
        {
            FightSingle t_single = t_obj as FightSingle;
            //t_single.GetIcon.alpha = 1;
            t_single.GetIcon.color = Color.white;
        }
    }

    #endregion
}

//玩家信息
public class FightData
{
    public string m_icon;   //头像
    public float m_blood;   //血量
    public string m_name;   //名字
    public List<FAnimalData> m_data_list;   //宠物信息

    public FightData()
    {
        m_data_list = new List<FAnimalData>();
    }
}

//战斗中的宠物信息
public class FAnimalData
{
    public enum FAnimalType
    {
        Attack = 0,     //攻击
        FuZhu = 1,      //辅助
        Health = 2,     //生命
        Null = 3,       //空
    }

    public FAnimalType m_animal_type;
    public float m_power_value;
    public float m_power_max_value;

    public FAnimalData()
    {
        m_animal_type = FAnimalType.Null;
    }
}


