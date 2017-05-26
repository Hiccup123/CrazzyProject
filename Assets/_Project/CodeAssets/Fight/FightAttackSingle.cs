using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;
using FairyGUI.Utils;

namespace Fight
{
    public class FightAttackSingle : GComponent
    {
        private GGraph m_effect1;
        private GGraph m_effect2;
        private GGraph m_end_effect;
        private SpineAnim m_end_anim;

        // 攻击目标.
        private Vector3 m_target_pos;
        private Vector3 m_start_pos;
        private EventDelegate m_end_event;
        private EventDelegate1 m_effect_end_event;
        private float m_time;
        private int m_pos_x;
        private int m_pos_y;
        private bool m_self;
        private bool m_move;

        private GameObject m_effect_obj1;
        private GameObject m_effect_obj2;

        private float m_cur_time = 0.5f;
        private bool m_five;

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_effect1 = GetChild("n0").asGraph;
            m_effect2 = GetChild("n1").asGraph;
            m_end_effect = GetChild("n2").asGraph;
            m_effect_obj1 = GameManager.LoadObj(m_effect1, Vector3.zero, "Effect/huo", Vector3.zero, Vector3.one * 5);
            m_effect_obj2 = GameManager.LoadObj(m_effect2, Vector3.zero, "Effect/flash", Vector3.zero, Vector3.one * 5);

            GameObject t_obj = GameManager.LoadObj(m_end_effect, Vector3.zero, "Effect/Effect", Vector3.zero, Vector3.one * 100);
            m_end_anim = t_obj.GetComponent<SpineAnim>();

            SetObjActive(false,false);
        }

        public void InItEffect(Vector3 p_end_pos, bool p_self,bool p_five,EventDelegate p_end_event, EventDelegate1 p_effect_end)
        {
            m_target_pos = p_end_pos;
            m_end_event = p_end_event;
            m_effect_end_event = p_effect_end;
            m_time = 0;
            m_start_pos = this.position;
            m_pos_x = Random.Range(0, 2) == 0 ? 150 : -150;
            m_pos_y = p_self ? 150 : -150;
            m_self = p_self;

            m_cur_time = 0.2f;
            m_five = p_five;
            SetObjActive(!m_five, m_five);
            m_move = true;
        }

        protected override void OnUpdate()
        {
            if (!m_move) return;

            m_time += m_cur_time;
            float t = m_time / (float)30;
            
            if(t > Random.Range(0.2f,0.5f))
            {
                m_cur_time = Random.Range(0.7f,1f);
            }

            this.position = ComData.Calculate(t, m_start_pos, m_start_pos + new Vector3(m_pos_x, m_pos_y), m_target_pos);

            if (m_self)
            {
                if (this.position.y <= m_target_pos.y)
                {
                    SetEnd();
                }
            }
            else
            {
                if (this.position.y >= m_target_pos.y)
                {
                    SetEnd();
                }
            }
        }

        void SetEnd ()
        {
            m_end_anim.PlayAnim("pet_hurteffect_" + (m_five ? "blue" : "red"),false);
            m_move = false;
            SetObjActive(false, false);
            m_end_event();
            m_effect_end_event(this);
        }

        void SetObjActive (bool p_value1,bool p_value2)
        {
            m_effect_obj1.SetActive(p_value1);
            m_effect_obj2.SetActive(p_value2);
        }

        /// <summary>
        /// 弃用
        /// </summary>
        //每秒最大可旋转的角度.
        private float m_max_rotation = 90;

        //每帧最大可旋转的角度.
        //private static float MAX_ROTATION_FRAME = MAX_ROTATION / ((float) (Application.targetFrameRate == -1 ? 60 : Application.targetFrameRate));
        private float m_max_rotation_frame;//m_max_rotation_frame = m_max_rotation / ((float)60);

        void Update()
        {
            //转向目标
            float t_x = m_target_pos.x - this.position.x;
            float t_y = m_target_pos.y - this.position.y;
            float t_rotate = Mathf.Atan2(t_y, t_x) * 180 / Mathf.PI;
            //得到最终的角度并且确保在 [0, 360) 这个区间内
            t_rotate -= 90;
            t_rotate = MakeSureRightRotation(t_rotate);
            //获取增加的角度
            float originRotationZ = MakeSureRightRotation(this.rotation);
            float addRotationZ = t_rotate - originRotationZ;
            //超过 180 度需要修改为负方向的角度
            if (addRotationZ > 180)
            {
                addRotationZ -= 360;
            }
            //不超过每帧最大可旋转的阀值
            addRotationZ = Mathf.Max(-m_max_rotation_frame, Mathf.Min(m_max_rotation_frame, addRotationZ));
            //应用旋转
            this.rotation = this.rotation + addRotationZ;
            //移动
            this.position = new Vector3(0, 5f * Time.deltaTime, 0);

            if (this.position == m_target_pos)
            {
                

                this.Dispose();
            }
        }

        /// <summary>
        /// 确保角度在 [0, 360) 这个区间内.
        /// </summary>
        /// <param name="rotation">任意数值的角度.</param>
        /// <returns>对应的在 [0, 360) 这个区间内的角度.</returns>
        private float MakeSureRightRotation(float rotation)
        {
            rotation += 360;
            rotation %= 360;
            return rotation;
        }
    }
}

