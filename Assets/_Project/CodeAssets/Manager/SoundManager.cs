using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using FairyGUI;

public class SoundManager {

    public static string S_SoundPath = "";
    
    public struct SoundData
    {
        public int m_id;
        public AudioClip m_clip;
    }

    public static Dictionary<int, SoundData> m_sound_data_dic = new Dictionary<int, SoundData>();

    public static void InItManager()
    {
        m_sound_data_dic.Clear();

        List<SoundListTemplate> t_temp_list = SoundListTemplate.m_templates;
        //Debug.Log("clip:" + t_temp_list.Count);
        for (int i = 0;i < t_temp_list.Count;i ++)
        {
            SoundData t_data = new SoundData();
            t_data.m_id = t_temp_list[i].id;
            t_data.m_clip = (AudioClip)Resources.Load("Sounds/" + t_temp_list[i].name);
            //Debug.Log("clip:" + t_data.m_clip);
            if(!m_sound_data_dic.ContainsKey(t_data.m_id))
            {
                m_sound_data_dic.Add(t_data.m_id, t_data);
            }
        }
    }

    public static AudioClip GetAudioClip (int p_id)
    {
        if(!m_sound_data_dic.ContainsKey(p_id))
        {
            Debug.LogError("Can not get AudioClip by id:" + p_id);
            return null;
        }
        return m_sound_data_dic[p_id].m_clip;
    }

    public static void Play(int p_id, AudioSource p_source, bool p_loop = false)
    {
        if(!m_sound_data_dic.ContainsKey(p_id))
        {
            return;
        }
        p_source.clip = m_sound_data_dic[p_id].m_clip;
        p_source.loop = p_loop;
        p_source.Play();
    }

    public static AudioSource SetAudioSource(GGraph p_graph)
    {
        GameObject t_audio_obj = GameObject.Instantiate(Resources.Load("ComPrefabs/AudioSource") as GameObject);
        t_audio_obj.transform.localPosition = Vector3.zero;
        t_audio_obj.transform.localScale = Vector3.one;
        p_graph.SetNativeObject(new GoWrapper(t_audio_obj));

        return t_audio_obj.GetComponent<AudioSource>();
    }
}
