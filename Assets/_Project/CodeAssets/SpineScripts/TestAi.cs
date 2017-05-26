using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TestAi : MonoBehaviour {

    private SkeletonAnimation m_anim;

    [SpineAnimation("Idle")]
    public string m_idle;
    [SpineAnimation]
    public string m_idle1;
    [SpineAnimation]
    public string m_idle2;
    [SpineAnimation]
    public string m_walk;
    [SpineAnimation]
    public string m_attack;
    [SpineSlot]
    public string m_eyeSlot;
    [SpineAttachment(currentSkinOnly: true,slotField:"eyesSlot")]
    public string m_eyeOpenAttachment;
    [SpineAttachment(currentSkinOnly: true, slotField: "eyesSlot")]
    public string m_blinkAttachment;

    [Range(0,0.2f)]
    public float m_blinkDuration = 0.05f;

    private float m_moveSpeed = 10;

    private bool m_is_idle = true;

    private AIAction m_action;
    public AIAction AI_Action { get { return m_action; } }

    private int m_show_id;
    public int Show_Id { get { return m_show_id; } set { m_show_id = value; } } //显示气泡id

    private Vector3 m_test_pos = new Vector3(200,-400,1000);

    void Awake()
    {
        m_anim = GetComponent<SkeletonAnimation>();
//        m_anim.OnRebuild += Apply;
    }

    void Start()
    {
        //m_action = AIAction.Idle;
        //StartCoroutine("IdleRanom");
        //m_anim.state.Event += StartAnim;
        m_show_id = -1;

		m_anim.skeleton.SetToSetupPose ();


		m_anim.state.SetAnimation (0, "rabbit_catch_1", false);
        m_anim.state.AddAnimation(0, "rabbit_catch_2", true, 0);
        //		m_anim.state.AddAnimation (0,"rabbit_tand_free_2",false,0);
        //		m_anim.state.AddAnimation (0,"rabbit_tand_free_3",false,0);
    }

    void Apply(SkeletonRenderer p_renderer)
    {
        //Debug.Log("Blink_");
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        while (true)
        {
            //Debug.Log("Blink");
            yield return new WaitForSeconds(Random.Range(0.25f, 3f));
            m_anim.skeleton.SetAttachment(m_eyeSlot, m_blinkAttachment);
            yield return new WaitForSeconds(m_blinkDuration);
            m_anim.skeleton.SetAttachment(m_eyeSlot, m_eyeOpenAttachment);
        }
    }

    IEnumerator IdleRanom()
    {
        while (m_action == AIAction.Idle)
        {
            //m_anim.state.SetAnimation(0, "hit", false);
            //spineboy.state.AddAnimation(0, "idle", true, 0);
            //spineboy.state.SetAnimation(0, "death", false).TrackEnd = float.PositiveInfinity;
            m_anim.skeleton.FlipX = Random.Range(0, 2) == 0 ? false : true;
            int p_random_idle = Random.Range(0, 4);
            Debug.Log("p_random_idle；" + p_random_idle);
            switch (p_random_idle)
            {
                case 0:
                    m_anim.state.SetAnimation(0, m_idle, true);
                    break;
                case 1:
                    m_anim.state.SetAnimation(0, m_idle1, false);
                    m_anim.state.AddAnimation(0, m_idle, true, 0);
                    break;
                case 2:
                    m_anim.state.SetAnimation(0, m_idle2, false);
                    m_anim.state.AddAnimation(0, m_idle, true, 0);
                    break;
                case 3:
                    float t_random_x = transform.localPosition.x + Random.Range(-200, 100);
                    float t_random_y = transform.localPosition.y + Random.Range(-100, 100);
                    m_test_pos = new Vector3(t_random_x, t_random_y, transform.localPosition.z);
                    m_action = AIAction.Walk;
                    break;
            }

            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
    }

    void Update()
    {
        //Action();
    }

    void Action()
    {
        if (m_action == AIAction.Idle)
        {
            if (m_is_idle)
            {
                StartCoroutine("IdleRanom");
                m_is_idle = false;
            }
        }
        else
        {
            StopCoroutine("IdleRanom");
            if (m_action == AIAction.Walk)
            {
                AnimalAction(m_walk, true);
                Walk(m_test_pos);
            }
            else
            {
                switch (m_action)
                {
                    case AIAction.Shit:
                        break;
                    case AIAction.Sleep:
                        break;
                    case AIAction.Eat:
                        break;
                    case AIAction.Beg_Food:
                        break;
                    case AIAction.Beg_Shower:
                        break;
                    case AIAction.Beg_Touch:
                        break;
                    case AIAction.Be_Bathed:
                        break;
                    case AIAction.Be_Caught:
                        break;
                    case AIAction.Be_Touched:
                        break;
                    case AIAction.Be_Beat_By_Other_Animals:
                        break;
                    case AIAction.Beat_Other_Animals:
                        break;
                    case AIAction.Medicine:
                        break;
                    case AIAction.Eat_Wrong_Medicine:
                        break;
                }

                AnimalAction (m_attack,false);
                AnimActionEnd();
            }
        }

        if (transform.localPosition != m_test_pos)
        {
            m_action = AIAction.Walk;
        }

        if (Input.GetKey (KeyCode.A))
        {
            m_action = AIAction.Medicine;
        }
    }

    #region Animation
    public void SetAnimation(AIAction p_action,bool p_loop = true)
    {
        m_action = p_action;
        switch (p_action)
        {
            case AIAction.Idle:
                AnimalAction(m_idle, p_loop);
                break;
            case AIAction.Walk:
                AnimalAction(m_walk, p_loop);
                break;
            case AIAction.Shit:
                AnimalAction(m_attack, p_loop);
                break;
            case AIAction.Sleep:
                AnimalAction(m_attack, p_loop);
                break;
            case AIAction.Eat:
                AnimalAction(m_attack, p_loop);
                break;
            case AIAction.Beg_Food:
                AnimalAction(m_idle1, p_loop);
                m_show_id = 0;
                break;
            case AIAction.Beg_Shower:
                AnimalAction(m_idle1, p_loop);
                m_show_id = 2;
                break;
            case AIAction.Beg_Touch:
                AnimalAction(m_idle2, p_loop);
                m_show_id = 1;
                break;
            case AIAction.Be_Bathed:
                AnimalAction(m_idle1, p_loop);
                break;
            case AIAction.Be_Caught:
                AnimalAction(m_attack, p_loop);
                break;
            case AIAction.Be_Touched:
                AnimalAction(m_idle2, p_loop);
                break;
            case AIAction.Be_Beat_By_Other_Animals:
                break;
            case AIAction.Beat_Other_Animals:
                break;
            case AIAction.Medicine:
                AnimalAction(m_idle2, p_loop);
                break;
            case AIAction.Eat_Wrong_Medicine:
                AnimalAction(m_idle1, p_loop);
                break;
        }
    }
    #endregion

    void AnimalAction(string p_anim_name,bool p_loop)
    {
        m_anim.state.SetAnimation(0, p_anim_name, p_loop);
    }

    void AnimActionEnd()
    {
        m_action = AIAction.Idle;
        m_is_idle = true;
    }

    void Walk(Vector3 p_pos)
    {
        m_anim.skeleton.FlipX = p_pos.x - transform.localPosition.x > 0 ? false : true;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, p_pos, m_moveSpeed);

        if (transform.localPosition == m_test_pos)
        {
            AnimActionEnd();
        }
    }
}
