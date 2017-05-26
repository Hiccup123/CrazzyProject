using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class FightEndUI : Window {

    private FightEndMsg m_msg;

    private Transition m_end_trans;
    private GLoader m_end_icon;

    private GGroup m_finish_group;

    private GLoader m_finish_icon;
    private Transition m_finish_trans;

    private GGroup m_victory_group;
    private GList m_reward_list;
    private GGroup m_stars_list;

    private GComponent m_player;
    private GGroup m_text_group;
    private GTextField m_name;
    private GTextField m_level;
    private GTextField m_exp;

    private GButton m_quit;
    private GButton m_again;

	public FightEndUI (FightEndMsg p_msg)
    {
        m_msg = p_msg;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("FightPack", "FightEnd").asCom;
        this.Center();
        this.modal = true;

        m_end_trans = this.contentPane.GetTransition("FightEnd");
        m_end_icon = this.contentPane.GetChild("FightEnd").asLoader;

        m_finish_group = this.contentPane.GetChild("JieSuan").asGroup;

        m_finish_icon = this.contentPane.GetChild("EndIcon").asLoader;
        m_finish_trans = this.contentPane.GetTransition("Victory");

        m_victory_group = this.contentPane.GetChild("Victory").asGroup;
        m_reward_list = this.contentPane.GetChild("RewardList").asList;
        m_stars_list = this.contentPane.GetChild("Stars").asGroup;

        m_player = this.contentPane.GetChild("Player").asCom;
        m_text_group = this.contentPane.GetChild("TextGroup").asGroup;
        m_name = this.contentPane.GetChild("Name").asTextField;
        m_level = this.contentPane.GetChild("Level").asTextField;
        m_exp = this.contentPane.GetChild("Exp").asTextField;

        m_quit = this.contentPane.GetChild("Btn_Quit").asButton;
        m_again = this.contentPane.GetChild("Btn_Again").asButton;

        m_quit.onClick.Add(Click);
        m_again.onClick.Add(Click);
        this.OnShown();
    }

    protected override void OnShown()
    {
        m_end_icon.visible = true;
        m_finish_group.visible = false;
        m_quit.visible = false;
        m_again.visible = false;
        m_end_trans.Play();
        m_end_trans.SetHook("EndTag",EndTransEnd);
    }

    void EndTransEnd()
    {
        Debug.Log("End");
        m_end_icon.visible = false;
        m_finish_group.visible = true;
        m_victory_group.visible = m_msg.m_end_id == 1;

        m_finish_icon.icon = UIPackage.GetItemURL("FightPack", "battle_text_" + (m_msg.m_end_id == 0 ? "shibai" : "xl"));
        m_finish_trans.Play();
        m_finish_trans.SetHook("Finished", FinishTransEvent);
    }

    void FinishTransEvent()
    {
        m_text_group.visible = m_msg.m_end_id == 1;
        m_reward_list.RemoveChildrenToPool();

        switch (m_msg.m_end_id)
        {
            case 0:

                FinishTransEnd();

                break;
            case 1:
                m_name.text = m_msg.m_name;
                m_exp.text = "经验值+" + m_msg.m_exp;
                m_level.text = "等级" + (m_msg.m_level - 1) + "->" + m_msg.m_level;

                Timers.inst.StartCoroutine(CreateReward());

                break;
        }
    }

    IEnumerator CreateReward()
    {
        int t_count = 0;
        for (int i = 0; i < 7; i++)
        {
            t_count++;
            GComponent t_reward = m_reward_list.AddItemFromPool().asCom;
            t_reward.GetChild("Btn_Check").visible = false;
            t_reward.GetChild("Icon").icon = UIPackage.GetItemURL("FightPack", "battle_text_baoxiang");
            yield return new WaitForSeconds(0.05f);
        }

        if(t_count >= 7)
        {
            FinishTransEnd();
        }
    }

    void FinishTransEnd()
    {
        m_quit.visible = true;
        m_again.visible = m_msg.m_end_id == 0;
    }

    void Click(EventContext p_context)
    {
        this.HideImmediately();
        string t_name = ((GObject)(p_context.sender)).name.Substring(4);
        Debug.Log("t_name:" + t_name);
        switch(t_name)
        {
            case "Quit":
                LevelManager.Instance.LoadLevel("MainScene", this.contentPane.position);
                break;
            case "Again":
                LevelManager.Instance.LoadLevel("FightScene", this.contentPane.position);
                break;
        }
    }
}

public class FightEndMsg
{
    public int m_end_id;    //0-失败 1-胜利
    public int m_stars;     //获得星数
    public int m_player_id; //玩家id
    public string m_name;   //玩家名字
    public int m_exp;    //经验
    public int m_level;  //等级
    public List<int> m_reward_list; //奖励list
}
