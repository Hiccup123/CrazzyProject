using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraShake : MonoBehaviour 
{
    public Transform m_camera;
    private Vector3 m_root_pos;
    private Vector3 m_cur_pos;
    private bool m_shake;

    void Start()
    {
        m_root_pos = m_camera.position;
        //Debug.Log(m_root_pos);
        m_shake = false;
    }

    public void SetCamera ()
    {
        m_camera.position = m_root_pos;
        Shake();
    }

    void Shake ()
    {
        //if (m_shake) return;
        Tweener t_tweener = m_camera.DOShakePosition(1, new Vector3(0.05f, 0.05f, 0));
        t_tweener.OnComplete(() =>
        {
            m_shake = true;
        });
    }

    //public Transform cameraTransform;
    //private Vector3 _currentPosition;		//记录抖动前的位置
    //private float _shakeCD = 0.002f;		//抖动的频率
    //private int _shakeCount = -1;			//设置抖动次数
    //private float _shakeTime;

    //void Start ()
    //{
    //	if(cameraTransform == null) cameraTransform = transform;

    //	_currentPosition = cameraTransform.position;	//记录抖动前的位置
    //	_shakeCount = Random.Range (50, 60);			//设置抖动次数
    //}

    //void Update ()
    //{
    //	if(_shakeTime + _shakeCD < Time.time && _shakeCount > 0)
    //	{
    //		_shakeCount --;
    //		float radio = Random.Range (-0.01f, 0.01f);

    //		if(_shakeCount == 1)	//抖动最后一次时设置为都动前记录的位置
    //			radio = 0;

    //		_shakeTime = Time.time;
    //		cameraTransform.position = _currentPosition + Vector3.one * radio;
    //	}
    //}
}
