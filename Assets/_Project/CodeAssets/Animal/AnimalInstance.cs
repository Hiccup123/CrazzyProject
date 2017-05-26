using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInstance<T> : MonoBehaviour where T : MonoBehaviour {

    public static T m_instance;

    public void Awake()
    {
        m_instance = (T)FindObjectOfType(typeof(T));
    }

    public void OnDestroy()
    {
        m_instance = null;
    }
}
