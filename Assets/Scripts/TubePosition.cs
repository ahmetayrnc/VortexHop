using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubePosition : MonoBehaviour
{
	private GameObject playerHolder;
	private bool destroyed;
	void Start()
	{
		playerHolder = GameManager.Instance.GetPlayerHolder();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (destroyed)
		{
			return;
		}
		
		if (transform.position.z < playerHolder.transform.position.z + 20f)
		{
			TubePooler.Instance.DestroyTube(gameObject,2f);
			TubePooler.Instance.CreateNextPlatform();
			destroyed = true;
		}
	}

	void OnEnable()
	{
		destroyed = false;
	}
}
