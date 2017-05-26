using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class ObjectPool {

    private Queue<GameObject> m_pool;
    private GameObject m_root;
    private GameObject m_parent;

    public ObjectPool (GameObject p_obj,int p_init_count)
    {
        m_root = p_obj;
        m_parent = new GameObject("EffetcParent");

        m_pool = new Queue<GameObject>(p_init_count);

        for(int i = 0;i < p_init_count;i ++)
        {
            GameObject t_obj = GameObject.Instantiate(m_root);
            t_obj.transform.parent = m_parent.transform;
            t_obj.transform.localScale = Vector3.one;
            t_obj.transform.localPosition = Vector3.zero;
            t_obj.SetActive(false);
            m_pool.Enqueue(t_obj);
        }
    }

    public GameObject GetObjFromPool ()
    {
        GameObject t_obj = null;
        if(m_pool.Count > 0)
        {
            t_obj = m_pool.Dequeue();
        }
        else
        {
            t_obj = GameObject.Instantiate(m_root);
            t_obj.transform.parent = m_parent.transform;
        }

        t_obj.SetActive(true);

        return t_obj;
    }

    public void Recycle (GameObject p_obj)
    {
        p_obj.SetActive(false);
        m_pool.Enqueue(p_obj);
    }

    public void Clear ()
    {
        foreach (GameObject obj in m_pool)
            GameObject.Destroy(obj);
        m_pool.Clear();
    }
}
