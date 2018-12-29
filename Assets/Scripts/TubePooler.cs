using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubePooler : MonoBehaviour {

	public static TubePooler Instance;

	public float tubeCount;
	public float tubeGapDistance;
	public Vector3 lastTubePosition;
	
	public List<GameObject> activeTubes;
	public List<GameObject> inactiveTubes;
	
	public GameObject tubePrefab;
	public Transform tubeParent;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () 
	{
		CreateInitialTubes();
		PlaceInitialTubes();
	}
	
	
	void CreateInitialTubes()
	{
		for (int i = 0; i < 30; i++)
		{
			inactiveTubes.Add(
				Instantiate(tubePrefab,tubeParent));
			inactiveTubes[i].SetActive(false);
		}
	}

	void PlaceInitialTubes()
	{
		Vector3 pos = Vector3.zero;
		for (int i = 0; i < tubeCount; i++)
		{
			pos.z = (i * tubeGapDistance) - 20;
			
			CreateTube(pos);
		}
	}
	
	void CreateTube( Vector3 _position)
	{
		GameObject tubeToBeCreated = inactiveTubes[0];
		tubeToBeCreated.SetActive(true);
		tubeToBeCreated.transform.position = _position;
		lastTubePosition = _position;
		inactiveTubes.RemoveAt(0);
		activeTubes.Add(tubeToBeCreated);
	}
	
	public void CreateNextPlatform()
	{
		Vector3 position = lastTubePosition;
		position.z += tubeGapDistance;
		CreateTube(position);
	}
	
	public void DestroyTube(GameObject platform, float time)
	{
		StartCoroutine(DestroyTubeCor(platform, time));
	}
	
	void DestroyTube(GameObject platform)
	{
		StartCoroutine(DestroyTubeCor(platform, 0));
	}

	IEnumerator DestroyTubeCor(GameObject platform, float time)
	{
		yield return new WaitForSeconds(time);

		int index = 0;
		for (int i = 0; i < activeTubes.Count; i++)
		{
			if (activeTubes[i] == platform)
			{
				index = i;
				break;
			}
		}
		
		GameObject platformToBeDisabled = activeTubes[index];
		platformToBeDisabled.SetActive(false);
		activeTubes.RemoveAt(index);
		inactiveTubes.Add(platformToBeDisabled);
	}
}
