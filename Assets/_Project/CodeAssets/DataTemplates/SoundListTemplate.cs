using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class SoundListTemplate : XmlLoadManager
{
    public int id;
    public string name;

    public static List<SoundListTemplate> m_templates = new List<SoundListTemplate>();

    public static void LoadTemplate()
    {
        m_templates.Clear();

        XmlReader t_reader = null;

        TextAsset t_asset = Resources.Load(XmlLoadManager.m_load_path + "SoundList") as TextAsset;

        t_reader = XmlReader.Create(new StringReader(t_asset.text));

        bool t_has_items = true;

        do
        {
            t_has_items = t_reader.ReadToFollowing("SoundList");

            if (!t_has_items)
            {
                break;
            }

            SoundListTemplate t_template = new SoundListTemplate();

            {
                t_reader.MoveToNextAttribute();
                t_template.id = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.name = t_reader.Value;
            }
            //Debug.Log(m_templates.Count);
            m_templates.Add(t_template);
        }
        while (t_has_items);
    }

    public static SoundListTemplate GetPetBaseTemplateById(int p_id)
    {
        foreach (SoundListTemplate t_template in m_templates)
        {
            if (t_template.id == p_id)
            {
                return t_template;
            }
        }
        Debug.Log("Can't get SoundListTemplate by Id:" + p_id);
        return null;
    }
}
