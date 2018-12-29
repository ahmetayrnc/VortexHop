using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public float fixedX;
	public float fixedY;
	public float offsetZ;
	public Transform target;

	private Vector3 position;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		position = target.position;
		position.x = fixedX;
		position.y = fixedY;
		position.z += offsetZ;
		transform.position = position;
	}
}
