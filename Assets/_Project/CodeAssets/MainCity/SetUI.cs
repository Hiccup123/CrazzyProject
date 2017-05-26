using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class SetUI : CommonUI {

    private Controller m_switch1;
    private Controller m_switch2;

    public SetUI()
    {

    }

    protected override void OnInit()
    {
        base.ComInit("MenuPack","SetPanel",true, OnClose);

        m_switch1 = this.contentPane.GetController("c1");
        m_switch2 = this.contentPane.GetController("c2");

        OnShown();
    }

    protected override void OnShown()
    {
        base.ComShow("shezhi");

        m_switch1.selectedIndex = 0;
        m_switch2.selectedIndex = 1;

        this.contentPane.GetChild("Switch1").onClick.Add(() => { SwitchState(1); });
        this.contentPane.GetChild("Switch2").onClick.Add(() => { SwitchState(2); });
        
        this.contentPane.GetChild("Btn_Quit_Game").onClick.Add(() => { Debug.Log("QuitGame"); });
    }

    void SwitchState(int p_state)
    {
        
    }

    protected override void DoShowAnimation()
    {
        base.ComShowAnimation(OnAnimComplete);
    }

    void OnAnimComplete()
    {

    }

    void OnClose()
    {

    }
}
