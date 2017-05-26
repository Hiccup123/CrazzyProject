using UnityEngine;
using System;
using System.Collections;

public class GameSingleton<T> : MonoBehaviour where T : MonoBehaviour{

	private static bool m_applicationIsQuitting;
	public static bool M_ApplicationIsQuitting
	{
		get { return m_applicationIsQuitting; }
	}

	private static readonly object temp = new object();

	private static T m_instance;

	public static T GetInstanceWithOutCreate ()
	{
		return m_instance;
	}

	/// <summary>
	/// get the instance of this type monobehaviour, will create a component attached to DontDestroyOnLoad if instance is null
	/// </summary>
	public static T Instance 
	{
		get
		{
			if (m_instance == null)
			{
				//lock to assure only instance once.
				lock (temp)
				{
					//another thread may wait outside the lock while this thread goes into lock and instanced singleton, so check null again when enter into lock.
					if (m_instance == null)
					{
						m_instance = (T)FindObjectOfType (typeof (T));
						
						if (FindObjectsOfType (typeof (T)).Length > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong " +
							               " - there should never be more than 1 singleton!" +
							               " Reopenning the scene might fix it.");
							
							return m_instance;
						}
						
//						var singleton = GameObjectHelper.GetDontDestroyOnLoadGameObject();
						
//						m_instance = singleton.AddComponent<T>();
					}
				}
			}

			return m_instance;
		}
	}

	public void OnDestroy ()
	{
		m_instance = null;
	}

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	/// it will create a buggy ghost object that will stay on the Editor scene
	/// even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// 
	/// </summary>
	/// 
	/// Instance will be destroyed manually in our game.
	public void OnApplicationQuit ()
	{
		m_applicationIsQuitting = true;
	}
}