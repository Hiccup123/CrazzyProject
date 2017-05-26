using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using System;

public class CSleep : CAIState {

    private CAIControl m_control;

    public CSleep (CAIControl p_control)
    {
        m_control = p_control;
        m_state_id = StateID.State_Sleep;
    }

    public override void ByCondition()
    {
        
    }

    public override void Action()
    {
        
    }
}
