using UnityEngine;
using System.Collections;

public class GameObjHelper {

	#region Temp GameObject

	private static GameObject m_temp_root_gb = null;

	private static string CONST_TEMP_ROOT_NAME = "_Temp_GB_Root";

	public static GameObject GetTempGBRoot ()
	{
		if (m_temp_root_gb == null)
		{
			m_temp_root_gb = new GameObject ();

			m_temp_root_gb.name = CONST_TEMP_ROOT_NAME;
		}

		return m_temp_root_gb;
	}

	#endregion

	#region Fx GameObject

	private static GameObject m_fx_root_gb = null;
	
	private static string CONST_FX_ROOT_NAME	= "_Fx_GB_Root";
	
	public static GameObject GetFxGBRoot()
	{
		if (m_fx_root_gb == null)
		{
			m_fx_root_gb = new GameObject();
			
			m_fx_root_gb.transform.parent = GetTempGBRoot().transform;
			
			m_fx_root_gb.name = CONST_FX_ROOT_NAME;
		}
		
		return m_fx_root_gb;
	}

	#endregion

	#region Dont_Destroy_On_Load

	public static GameObject m_dont_gb = null;

	private static string CONST_DONT_DESTROY_ON_LOAD_GB_NAME = "_Dont_Destroy_On_Load";

	public static string GetDontDestroyGBName ()
	{
		return CONST_DONT_DESTROY_ON_LOAD_GB_NAME;
	}

	/// <summary>
	/// Get DontDestroyOnLoad GB
	/// </summary>
	public static GameObject GetDontDestroyOnLoadGameObject ()
	{
		if (m_dont_gb == null)
		{
			m_dont_gb = new GameObject();

			m_dont_gb.name = CONST_DONT_DESTROY_ON_LOAD_GB_NAME;

			DontDestroyGB (m_dont_gb);
		}

		return m_dont_gb;
	}

	/// <summary>
	/// Clears the dont destroy G.
	/// </summary>
	public static void ClearDontDestroyGB ()
	{
		if (m_dont_gb != null)
		{
			MonoBehaviour[] t_items = m_dont_gb.GetComponents<MonoBehaviour> ();

			for (int i = 0;i < t_items.Length;i ++)
			{
				//TODO:uncreate any codemanager tools
//				if( t_items[i].GetType() == typeof(UtilityTool) ||
//				   t_items[i].GetType() == typeof(ConfigTool) ||
//				   t_items[i].GetType() == typeof(Bundle_Loader) ||
//				   t_items[i].GetType() == typeof(BundleHelper) ||
//				   t_items[i].GetType() == typeof(ThirdPlatform) ||
//				   t_items[i].GetType() == typeof(UIRootAutoActivator) ){
//					continue;
//				}

				{
					t_items[i].enabled = false;
					
					GameObject.Destroy( t_items[i] );
				}
			}
		}
	}

	#endregion

	#region DontDestroy

	public static void DontDestroyGB (GameObject p_gb)
	{
		if (p_gb == null)
		{
			Debug.LogError ("p_gb is null.");

			return;
		}

		GameObject.DontDestroyOnLoad (p_gb);
	}
	
	#endregion

	#region Hierarchy
	//TODO:To UnderStand This Code 
	public static void LogGameObjectHierarchy (GameObject p_gb,string p_prefex = "")
	{
		if (p_gb == null)
		{
			Debug.Log ("p_gb is null.");

			return;
		}

		string t_origin_name = p_gb.name;

		Debug.Log (p_prefex + " " + t_origin_name + " " + GetGBHierarchy (p_gb));
	}

	public static string GetGBHierarchy (GameObject p_gb)
	{
		if (p_gb == null)
		{
			return "<Null GameObject>";
		}

		string t_name = p_gb.name;

		while (p_gb.transform.parent != null)
		{
			t_name = p_gb.transform.parent.name + "/" + t_name;

			p_gb = p_gb.transform.parent.gameObject;
		}

		return t_name;
	}

	public static string GetGBHierarchy (Component p_com)
	{
		if (p_com == null)
		{
			return "<Null Component>";
		}
		
		GameObject t_gb = p_com.gameObject;
		
		if (t_gb == null)
		{
			return "<Null GameObject>";
		}
		
		string t_name = t_gb.name;
		
		while (t_gb.transform.parent != null)
		{
			t_name = t_gb.transform.parent.name + "/" + t_name;
			
			t_gb = t_gb.transform.parent.gameObject;
		}
		
		return t_name;
	}

	#endregion

	#region GB Helper

	public static GameObject GetChild (GameObject p_gb,string p_child_name)
	{
		if (p_gb == null)
		{
			Debug.LogError ("Error,GameObject is null.");

			return null;
		}

		int t_count = p_gb.transform.childCount;

		for (int i = 0;i < t_count;i ++)
		{
			Transform t_trans = p_gb.transform.GetChild (i);

			if (t_trans == null)
			{
				continue;
			}

			if (t_trans.name == p_child_name)
			{
				return t_trans.gameObject;
			}
			else
			{
				GameObject t_gb = GetChild (t_trans.gameObject,p_child_name);

				if (t_gb != null)
				{
					return t_gb;
				}
			}
		}

		return null;
	}

	public static GameObject GetRootGB (GameObject p_gb)
	{
		if (p_gb == null)
		{
			Debug.LogError ("Error,Root GB is null.");

			return null;
		}

		while (p_gb.transform.parent != null)
		{
			p_gb = p_gb.transform.parent.gameObject;
		}

		return p_gb;
	}

	/// <summary>
	/// Hides the and destroy gameobject.
	/// Notes:
	/// 1.if in PlayMode, destroy it;
	/// 2.if in EditMode, destroyimmediately, since destroy is notusable in EditMode;
	/// </summary>
	public static void HideAndDestroy (GameObject p_target)
	{
		if (p_target == null)
		{
			Debug.LogError ("p_target is null.");

			return;
		}

		p_target.SetActive (false);
		
		if (Application.isPlaying)
		{
			GameObject.Destroy (p_target);
		}
		else
		{
			GameObject.DestroyImmediate (p_target);
		}
	}

	#endregion

	#region Mono
	
	public static bool HaveMissingMono (GameObject t_gb)
	{
		MonoBehaviour[] t_monos = t_gb.GetComponents<MonoBehaviour>();
		
		for (int i = 0; i < t_monos.Length; i++)
		{
			if (t_monos[ i ] == null)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public static void LogComponents (GameObject p_gb)
	{
		MonoBehaviour[] t_monos = p_gb.GetComponentsInChildren<MonoBehaviour>();
		
		for (int i = 0; i < t_monos.Length; i++)
		{
			MonoBehaviour t_mono = t_monos[ i ];
			
			if (t_mono != null)
			{
				Debug.Log (i + " : " + t_monos[ i ] + " - " + t_mono.GetType ());
			}
			else
			{
				Debug.Log (i + " : " + t_monos[ i ] + " is null.");
			}
		}
	}
	
	#endregion
}