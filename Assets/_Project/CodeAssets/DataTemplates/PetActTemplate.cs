using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class PetActTemplate : XmlLoadManager
{
    public int id;
    public string Stand;
    public string Walk;
    public string Sleep;
    public string Eat;
    public string Shit;
    public string WEat;
    public string WStroke;
    public static List<PetActTemplate> m_templates = new List<PetActTemplate>();

    public Dictionary<AIAction, List<string>> m_action_dic = new Dictionary<AIAction, List<string>>();

    public static void LoadTemplate()
    {
        m_templates.Clear();
        
        XmlReader t_reader = null;

        TextAsset t_asset = Resources.Load(XmlLoadManager.m_load_path + "PetAct") as TextAsset;

        t_reader = XmlReader.Create(new StringReader(t_asset.text));

        bool t_has_items = true;

        do
        {
            t_has_items = t_reader.ReadToFollowing("PetAct");

            if (!t_has_items)
            {
                break;
            }

            PetActTemplate t_template = new PetActTemplate();
            {
                t_reader.MoveToNextAttribute();
                t_template.id = int.Parse(t_reader.Value);
                
                t_reader.MoveToNextAttribute();
                t_template.Stand = t_reader.Value;
            }
            //Debug.Log(m_templates.Count);
            m_templates.Add(t_template);
        }
        while (t_has_items);
    }

    public static PetActTemplate GetPetBaseTemplateById(int p_id)
    {
        foreach (PetActTemplate t_template in m_templates)
        {
            if (t_template.id == p_id)
            {
                return t_template;
            }
        }
        Debug.Log("Can't get PetActTemplate by Id:" + p_id);
        return null;
    }
}
