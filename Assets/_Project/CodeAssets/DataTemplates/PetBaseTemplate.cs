using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class PetBaseTemplate : XmlLoadManager
{
    public int id;
    public string NAME;
    public string BodyPic;
    public string HeadPic;
	public string ActLead;
    public int LEVAL;
    public int NGID;
    public int TYPEID;
    public int HP;
    public int PORCE;
    public int CHARM;
    public int SKILL1;
    public int SKILL2;
    public int SKILL3;
    public int EQUIP1;
    public int EQUIP2;
    public int EQUIP3;
    public int EQUIP4;
    public int ORNA1;
    public int ORNA2;
    public int ORNA3;
    public int ORNA4;

    public static List<PetBaseTemplate> m_templates = new List<PetBaseTemplate>();

    public static void LoadTemplate()
    {
        m_templates.Clear();

        XmlReader t_reader = null;

        TextAsset t_asset = Resources.Load(XmlLoadManager.m_load_path + "PetBase") as TextAsset;

        t_reader = XmlReader.Create(new StringReader(t_asset.text));

        bool t_has_items = true;

        do
        {
            t_has_items = t_reader.ReadToFollowing("PetBase");

            if (!t_has_items)
            {
                break;
            }

            PetBaseTemplate t_template = new PetBaseTemplate();

            {
                t_reader.MoveToNextAttribute();
                t_template.id = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.NAME = t_reader.Value;

                t_reader.MoveToNextAttribute();
                t_template.BodyPic = t_reader.Value;

                t_reader.MoveToNextAttribute();
                t_template.HeadPic = t_reader.Value;

				t_reader.MoveToNextAttribute();
				t_template.ActLead = t_reader.Value;

                t_reader.MoveToNextAttribute();
                t_template.LEVAL = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.NGID = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.TYPEID = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.HP = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.PORCE = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.CHARM = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.SKILL1 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.SKILL2 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.SKILL3 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.EQUIP1 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.EQUIP2 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.EQUIP3 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.EQUIP4 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.ORNA1 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.ORNA2 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.ORNA3 = int.Parse(t_reader.Value);

                t_reader.MoveToNextAttribute();
                t_template.ORNA4 = int.Parse(t_reader.Value);
            }
            //Debug.Log(m_templates.Count);
            m_templates.Add(t_template);
        }
        while (t_has_items);
    }

    public static PetBaseTemplate GetPetBaseTemplateById(int p_id)
    {
        foreach (PetBaseTemplate t_template in m_templates)
        {
            if (t_template.id == p_id)
            {
                return t_template;
            }
        }
        Debug.Log("Can't get PetBaseTemplate by Id:" + p_id);
        return null;
    }
}
