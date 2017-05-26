using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

namespace Fight
{
    public class SpineAnim : MonoBehaviour
    {
        private SkeletonAnimation m_anim;
        public GameObject m_spine_root;

        private Dictionary<string,EventDelegate> m_event_dic = new Dictionary<string, EventDelegate>();

        void Start()
        {
            m_anim = m_spine_root.GetComponent<SkeletonAnimation>();
            m_anim.state.Event += SpineEvent;
            //m_anim.state.SetAnimation(0,"pet_arrow",true);
            m_spine_root.SetActive(false);
        }

        void SpineEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (!m_event_dic.ContainsKey(e.Data.Name)) return;

            if (e.Data.Name == "end")
            {
                //Debug.Log("End");
                m_spine_root.SetActive(false);
            }

            if (m_event_dic[e.Data.Name] != null)
            {
                m_event_dic[e.Data.Name]();
            }
        }

        //设置帧事件
        public void SetEvent(string p_name, EventDelegate p_delegate)
        {
            if(!m_event_dic.ContainsKey(p_name))
            {
                m_event_dic.Add(p_name, p_delegate);
            }
            else
            {
                m_event_dic[p_name] = p_delegate;
            }
        }

        //播放动画
        public void PlayAnim(string p_name, bool p_loop)
        {
            m_spine_root.SetActive(true);
            m_anim.state.SetAnimation(0, p_name, p_loop);
        }

        //添加动画
        public void AddAnima(string p_name, bool p_loop)
        {
            m_anim.state.AddAnimation(0, p_name, p_loop, 0);
        }

        //停止动画
        public void StopAnim()
        {
            m_spine_root.SetActive(false);
        }
    }
}

