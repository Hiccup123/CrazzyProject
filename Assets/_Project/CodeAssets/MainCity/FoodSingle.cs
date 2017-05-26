using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class FoodSingle : GButton {
    
    private GTextField m_item_name;
    private GTextField m_item_num;
    
    private GImage m_num_bg;

    private FoodItem m_food_item;

    public override void ConstructFromXML(XML cxml)
    {
        base.ConstructFromXML(cxml);
        
        m_item_name = GetChild("ItemName").asTextField;
        m_item_num = GetChild("Num").asTextField;
        m_num_bg = GetChild("NumBg").asImage;

        LongPressGesture gesture = new LongPressGesture(this);
        gesture.once = true;
        gesture.trigger = 0.5f;
        gesture.onAction.Add(OnLongPress);

        //this.draggable = true;
        //this.onDragStart.Add((EventContext context) =>
        //{
        //    //Cancel the original dragging, and start a new one with a agent.
        //    context.PreventDefault();

        //    DragDropManager.inst.StartDrag(this, this.icon, this.icon, (int)context.data);
        //});
    }

    void OnLongPress(EventContext p_context)
    {
        DragDropManager.inst.StartDrag(this, this.icon, this.icon);
    }

    public void InItItem(FoodItem p_item,int p_index,int p_count)
    {
        m_food_item = p_item;
        this.icon = UIPackage.GetItemURL("FoodPack", (p_index == p_count - 1 ? "food_btn_" : "food_text_") + p_item.m_icon);
        m_item_name.text = p_item.m_name;
        this.GetChild("Num").text = p_item.m_num == 0 ? "" : p_item.m_num.ToString();
        m_num_bg.visible = !string.IsNullOrEmpty(p_item.m_name);
    }
}
