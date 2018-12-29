using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPass : MonoBehaviour
{
	public static LevelPass Instance;
	public GameObject levelPassPrefab;
	
	GameObject levelPass;
	private LevelPassDestruction levelPassDestruction;
	
	void Awake()
	{
		Instance = this;
	}
	
	void Start()
	{
		levelPass = Instantiate(levelPassPrefab);
		
		levelPass.transform.position = Vector3.forward * 12.5f;

		levelPassDestruction = levelPass.GetComponent<LevelPassDestruction>();
		
		PlaceNewLevelPass();
	}
	
	public void PlaceNewLevelPass()
	{
		Vector3 newPos = levelPass.transform.position;
		newPos.z += GameManager.Instance.levelMilestone * PlatformPooler.Instance.platformGapDistance;
		levelPass.transform.position = newPos;

		var color = GameManager.Instance.newColor;
		color.a = 0.5f;

		levelPass.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;

		var renderers = levelPass.GetComponentsInChildren<MeshRenderer>();

		for (int i = 0; i < renderers.Length; i++) {
			renderers[i].material.color = color;
		}

        levelPassDestruction.RestoreLevelPass();
	}

	public void DestroyLevelPass()
	{
		levelPassDestruction.DestroyLevelPass();
	}
}
