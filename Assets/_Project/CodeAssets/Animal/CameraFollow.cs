using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float x_margin = 1f;	//Distance in the x axis the player can move before the camera follows.
	public float y_margin = 1f;	//Distance in the y axis the player can move before the camera follows.
	public float x_smooth = 8f;	//How smoothy the camera catches up with it's target movement in the x axis.
	public float y_smooth = 8f;	//How smoothy the camera catches up with it's target movement in the y axis.
	public Vector2 m_max_x_and_y;	//The maximum x and y coordinates the camera can have.
	public Vector2 m_min_x_and_y;	//The minimum x and y coordinates the camera can have.

	private Transform m_player;	//Reference to the player's transform

	void Awake ()
	{
		m_player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	bool CheckXMargin ()
	{
		//Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs (transform.position.x - m_player.position.x) > x_margin;
	}

	bool CheckYMargin ()
	{
		//Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs (transform.position.y - m_player.position.y) > y_margin;
	}

	void FixedUpdate ()
	{
		TrackPlayer ();
	}

	void TrackPlayer ()
	{
		//By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float t_target_x = transform.position.x;
		float t_target_y = transform.position.y;

		//If the player has moved beyond the x margin...
		if (CheckXMargin ())
		{
			//... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			t_target_x = Mathf.Lerp (transform.position.x,m_player.position.x,x_smooth * Time.deltaTime);
		}

		//If the player has moved beyond the y margin...
		if (CheckYMargin ())
		{
			//... the target y coordinate should be a Lerp between the camera's current y position and the player's current y positon.
			t_target_y = Mathf.Lerp (transform.position.y,m_player.position.y,y_smooth * Time.deltaTime);
		}

		//The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		t_target_x = Mathf.Clamp (t_target_x,m_min_x_and_y.x,m_max_x_and_y.x);
		t_target_y = Mathf.Clamp (t_target_y,m_min_x_and_y.y,m_max_x_and_y.y);

		//Set the camera's position to the target position with the same z component.
		transform.position = new Vector3 (t_target_x,t_target_y,transform.position.z);
	}
}
