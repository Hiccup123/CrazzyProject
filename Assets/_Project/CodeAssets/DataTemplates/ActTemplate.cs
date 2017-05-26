using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class ActTemplate : XmlLoadManager {

    public int ID;

    public string ACT_NAME;

    public int CLASS;

    public int CONDITION;

    public static List<ActTemplate> m_templates = new List<ActTemplate>();

    public static void LoadTemplate()
    {
        m_templates.Clear();

        XmlReader t_reader = null;

        TextAsset t_asset = Resources.Load(XmlLoadManager.m_load_path + "ACT") as TextAsset;

        t_reader = XmlReader.Create(new StringReader(t_asset.text));

        bool t_has_items = true;

        do
        {
            t_has_items = t_reader.ReadToFollowing("ACT");

            if (!t_has_items)
            {
                break;
            }

            ActTemplate t_template = new ActTemplate();

            {
                t_reader.MoveToNextAttribute();
                t_template.ID = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.ACT_NAME = t_reader.Value;

                t_reader.MoveToNextAttribute();
                t_template.CLASS = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.CONDITION = int.Parse(t_reader.Value);

            }
            //Debug.Log(m_templates.Count);
            m_templates.Add(t_template);
        }
        while (t_has_items);
    }

    public static ActTemplate GetActTemplateById(int p_id)
    {
        foreach (ActTemplate t_template in m_templates)
        {
            if (t_template.ID == p_id)
            {
                return t_template;
            }
        }
        Debug.Log("Can't get ActTemplate by Id:" + p_id);
        return null;
    }
}
