using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

public class PetListUI : CommonUI {

    private GList m_pet_list;

    public PetListUI()
    {

    }

    protected override void OnInit()
    {
        base.ComInit("PetCollectPack", "PetListUI", true, OnClose);

        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL ("PetCollectPack","PetListItem"),typeof (PetListSingle));
        //UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL ("PetMsgPack",""));

        m_pet_list = this.contentPane.GetChild("PetList").asList;

        OnShown();
    }

    protected override void OnShown()
    {
        base.ComShow("liebiao");

        m_pet_list.RemoveChildrenToPool();
        int p_item_count = 7;
        for (int i = 0;i < p_item_count;i ++)
        {
            PetListSingle t_item = m_pet_list.AddItemFromPool() as PetListSingle;
            t_item.InItItem();
        }
    }

    protected override void DoShowAnimation()
    {
        base.ComShowAnimation (OnAnimComplete);
    }

    void OnAnimComplete()
    {

    }

    void OnClose()
    {

    }
}
