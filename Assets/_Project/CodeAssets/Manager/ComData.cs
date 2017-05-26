using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using FairyGUI;

public class ComData {

    public static string GetUrl(string p_str_path)
    {
        return p_str_path.Substring(0,p_str_path.IndexOf ("."));
    }

    //宠物icon
    public static string GetIcon(int p_id)
    {
        switch (p_id)
        {
            case 4:
                return "shu";
            case 3:
                return "ji";
            case 2:
                return "gou";
            case 1:
                return "hong";
            case 5:
                return "zi";
            case 6:
                return "mao";
        }
        return "";
    }

    //获得颜色信息
    public static string GetColor(int p_id, string p_color_name)
    {
        string t_str = "battle_text_";
        switch (p_color_name)
        {
            case "Bg":
                switch (p_id)
                {
                    case 0:
                        t_str += "hongdi";
                        break;
                    case 1:
                        t_str += "landi";
                        break;
                    case 2:
                        t_str += "huangdi";
                        break;
                    case 3:
                        t_str += "lvdi";
                        break;
                }
                break;
            case "BL":
                switch (p_id)
                {
                    case 0:
                        t_str += "hongnl";
                        break;
                    case 1:
                        t_str += "lannl";
                        break;
                    case 2:
                        t_str += "huangnl";
                        break;
                    case 3:
                        t_str += "lvnl";
                        break;
                }
                break;
            case "Bg_Bar":
                switch (p_id)
                {
                    case 0:
                        t_str += "hongtiao";
                        break;
                    case 1:
                        t_str += "lantiao";
                        break;
                    case 2:
                        t_str += "huangtiao";
                        break;
                    case 3:
                        t_str += "lvtiao";
                        break;
                }
                break;

        }

        return UIPackage.GetItemURL("FightPack", t_str);
    }

    #region Move
    public static Vector3[] GetTransPos(Vector3 p_start_pos, Vector3 p_end_pos)
    {
        int t_count = 50;
        Vector3[] t_path = new Vector3[t_count];
        for (int i = 0; i < t_count; i++)
        {
            float t = i / (float)t_count;
            t_path[i] = Calculate(t, p_start_pos, p_end_pos + new Vector3(-50, 50, 0), p_end_pos);
        }
        return t_path;
    }

    public static Vector3 Calculate(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    public static Vector3 Calculate1(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float uu = u * u;
        float uuu = u * u * u;
        float tt = t * t;
        float ttt = t * t * t;

        Vector3 p = p0 * uuu + 3 * p1 * t * uu + 3 * p2 * tt * u + p3 * ttt;
        return p;
    }
    #endregion
}
