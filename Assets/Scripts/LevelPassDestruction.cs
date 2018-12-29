using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPassDestruction : MonoBehaviour
{
	public GameObject regular;
	public GameObject destroyed;

	public Vector3[] originalPositions;

	void Awake()
	{
		originalPositions = new Vector3[destroyed.transform.childCount];
		
		for (int i = 0; i < destroyed.transform.childCount; i++)
		{
			originalPositions[i] = destroyed.transform.GetChild(i).localPosition;
		}
	}
	
	void Start()
	{
		RestoreLevelPass();
	}
	
	public void DestroyLevelPass()
	{
		regular.SetActive(false);
		destroyed.SetActive(true);
		
		for (int i = 0; i < destroyed.transform.childCount; i++)
		{
			destroyed.transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(2000f,destroyed.transform.position,3f);
		}
		
	}

	public void RestoreLevelPass()
	{	
		for (int i = 0; i < destroyed.transform.childCount; i++)
		{
			destroyed.transform.GetChild(i).localPosition = originalPositions[i];
			destroyed.transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		
		regular.SetActive(true);
		destroyed.SetActive(false);
	}
}
