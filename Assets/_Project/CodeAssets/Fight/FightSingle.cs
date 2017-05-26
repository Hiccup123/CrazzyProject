using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

namespace Fight
{
    public class FightSingle : GComponent
    {
        private GLoader m_icon;
        public GLoader GetIcon { get { return m_icon; } }

        private AudioSource m_audio;

        private GGraph m_graph;
        private SpineAnim m_spine_anim;

        private int m_type;
        public int Type { get { return m_type; } set { m_type = value; } }

        private bool m_even;
        public bool Even { get { return m_even; } set { m_even = value; } } //是否为偶数列

        private bool m_choose = false;  //是否被消除
        public bool Choose { set { m_choose = value; } get { return m_choose; } }

        private bool m_have_check = false;  //是否已经被标记检测
        public bool HaveCheck { get { return m_have_check; } set { m_have_check = value; } }

        private BoxIndex m_box_index;       //item 下标
        public BoxIndex Box_Index { get { return m_box_index; } set { m_box_index = value; } }

        private int m_match_count;      //相互匹配次数
        public int MatchCount { get { return m_match_count; } set { m_match_count = value; } }

        private EventDelegate m_scale_delegate; //缩放回调
        private EventDelegate m_end_delegate;   //动画播放完毕回调

        public override void ConstructFromXML(XML cxml)
        {
            base.ConstructFromXML(cxml);

            m_icon = GetChild("n0").asLoader;

            GGraph t_graph = GetChild("Audio").asCom.GetChild("Audio").asGraph;
            m_audio = SoundManager.SetAudioSource(t_graph);
            m_graph = GetChild("Effect").asGraph;

            m_spine_anim = GameManager.LoadObj(m_graph,Vector3.zero, "Effect/Effect", new Vector3(35, -35), Vector3.one * 100).GetComponent<SpineAnim>();
        }

        //设置icon
        public void SetIcon(int p_id)
        {
            //Debug.Log("p_icon:" + p_id);
            m_type = p_id;
            m_icon.icon = UIPackage.GetItemURL("FightPack", "battle_text_" + ComData.GetIcon(p_id));
        }

        /// <summary>
        /// 特效播放
        /// </summary>
        /// <param name="p_five">是否消除5个（以上）</param>
        /// <param name="p_scale_delegate">缩放回调</param>
        /// <param name="p_end_delegate">本次操作结束回调</param>
        public void PlaySet(bool p_five, EventDelegate p_scale_delegate,EventDelegate p_end_delegate)
        {
            m_scale_delegate = p_scale_delegate;
            m_end_delegate = p_end_delegate;

            if (!p_five)
            {
                m_scale_delegate();
                m_spine_anim.SetEvent("end", m_end_delegate);
                m_spine_anim.PlayAnim("pet_effect_disappear",false);
            }
            else
            {
                m_spine_anim.SetEvent("end", FiveAnim);
                m_spine_anim.PlayAnim("pet_gatherlight", false);
            }
        }

        void FiveAnim ()
        {
            m_scale_delegate();
            m_spine_anim.SetEvent("end", m_end_delegate);
            m_spine_anim.PlayAnim("pet_effect_disappear", false);
        }

        //声音播放
        public void SoundPlay(int p_id)
        {
            SoundManager.Play(p_id, m_audio);
        }

        protected override void OnUpdate()
        {
            //Debug.Log("update");
            if (GRoot.inst.touchTarget != null)
            {
                if (GRoot.inst.touchTarget.parent != null && GRoot.inst.touchTarget.parent.name.Length > 5 && GRoot.inst.touchTarget.parent.name.Substring(0, 5) == "FItem")
                {
                    if (GRoot.inst.touchTarget.parent.name == this.name)
                    {
                        //FightUI.S_Hover_Obj = this;
                        FightMain.S_Hover_Obj = this;
                    }
                }
            }
        }
    }
}

