using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour
{
	public static PlatformPooler Instance;

	public float initialPlatformCount;
	public float platformGapDistance;
	public Vector3 lastPlatformPosition;
	public Vector3 lastPlatformRotation;
	
	public List<GameObject> activePlatforms;
	public List<GameObject> inactivePlatforms;

	public GameObject platformPrefab;
	public Transform platformParent;

	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () 
	{
		CreateInitialPlatforms();
		PlaceInitialPlatforms();
	}

	void CreateInitialPlatforms()
	{
		for (int i = 0; i < 30; i++)
		{
			inactivePlatforms.Add(
				Instantiate(platformPrefab,platformParent));
			inactivePlatforms[i].SetActive(false);
		}
	}

	void PlaceInitialPlatforms()
	{
		Vector3 pos = Vector3.zero;
		Vector3 rot = Vector3.zero;
		for (int i = 0; i < 4; i++)
		{
			pos.z = i * platformGapDistance;
			
			CreatePlatform(pos,rot, true);
		}

		for (int i = 0; i < initialPlatformCount; i ++)
		{
			CreateNextPlatform();
		}
	}

	public void DestroyPlatform(GameObject platform, float time)
	{
		StartCoroutine(DestroyPlatformCor(platform, time));
	}
	
	void DestroyPlatform(GameObject platform)
	{
		StartCoroutine(DestroyPlatformCor(platform, 0));
	}

	IEnumerator DestroyPlatformCor(GameObject platform, float time)
	{
		// Wait the wanted amount to destroy te platform
		yield return new WaitForSeconds(time);

		// Finds the platform to be destoryed
		int index = 0;
		for (int i = 0; i < activePlatforms.Count; i++)
		{
			if (activePlatforms[i] == platform)
			{
				index = i;
				break;
			}
		}
		
		// Gets the platform to be destroyed
		GameObject platformToBeDisabled = activePlatforms[index];
		
		// Disables the platform
		platformToBeDisabled.SetActive(false);
		
		// Gets the platfrom component for future uses
		Platform _platform = platformToBeDisabled.GetComponent<Platform>();
		
		// Resets the platform values
		_platform.ResetEverything();
		
		// Removes the platform from the active list and add to the inactive list
		activePlatforms.RemoveAt(index);
		inactivePlatforms.Add(platformToBeDisabled);
	}

	void CreatePlatform( Vector3 _position, Vector3 _rotation, bool dullPlatform )
	{
		// Gets the first platform from the inactive list
		GameObject platformToBeCreated = inactivePlatforms[0];
		
		// Activates the platform applies the rotation anf the position
		platformToBeCreated.SetActive(true);
		platformToBeCreated.transform.position = _position;
		platformToBeCreated.transform.rotation = Quaternion.Euler(_rotation);

		// Gets the platform component for future applications
		Platform _platform = platformToBeCreated.GetComponent<Platform>();
		
		// Picks a random value between 0 - 100
		float randValue = Random.Range(0f, 100f);

		// if it is a dull platform dont change anything
		if (!dullPlatform)
		{
			// There is a 12.5 percent chance that the platform will have a perfect
			if (randValue <= 12.5f)
			{
				_platform.SetPerfect();
			}
		
			// There is a 5 percent chance that the platform will be able to move
			if (randValue <= PlayerStats.levelHardnessMultiplier)
			{
				_platform.SetMoveable();
			}
		
			// There is a 10 percent chance that the platform will be at a random size
			if (randValue <= 50f + PlayerStats.levelHardnessMultiplier)
			{
				_platform.RandomSize();
			}
		}
		
		// Saves the platforms rotation and the position for future uses
		lastPlatformPosition = _position;
		lastPlatformRotation = _rotation;
		
		// Removes the platform from the inactive list and add to the active list
		inactivePlatforms.RemoveAt(0);
		activePlatforms.Add(platformToBeCreated);
	}

	public void CreateNextPlatform()
	{
		Vector3 position = lastPlatformPosition;
		Vector3 rotation = lastPlatformRotation;
		bool dullPlatform = false;
		
		// Picks a random angle for the platform
		int randomAngleMinMax = 60 + (int)(2.5f * PlayerStats.levelHardnessMultiplier);

		randomAngleMinMax = Mathf.Clamp(randomAngleMinMax, 0, 360);
		
		int randomAngle = Random.Range(-randomAngleMinMax, randomAngleMinMax);
		rotation.z = randomAngle;
		
		position.z += platformGapDistance;

		// This makes it so that first 4 platform in each level is in the middle
		if (  (position.z / platformGapDistance) % GameManager.Instance.levelMilestone  < 4f || position.z / platformGapDistance % GameManager.Instance.levelMilestone > GameManager.Instance.levelMilestone - 1)
		{
			rotation = Vector3.zero;
			dullPlatform = true;
		}
		
		// Creates the wanted platform
		CreatePlatform(position, rotation, dullPlatform);
	}
}
