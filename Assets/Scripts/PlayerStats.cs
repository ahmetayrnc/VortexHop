using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public static int score;
	public static int best;
	public static int multiplier;
	public static int level;
	public static int platformsHopped;
	public static int levelHardnessMultiplier;
	
	void Start()
	{
		score = 0;
		multiplier = 1;
		platformsHopped = 0;
		
		if (!PlayerPrefs.HasKey("best"))
		{
			PlayerPrefs.SetInt("best",0);
			PlayerPrefs.Save();
		}
		
		if (!PlayerPrefs.HasKey("level"))
		{
			PlayerPrefs.SetInt("level",1);
			PlayerPrefs.Save();
		}

		best = PlayerPrefs.GetInt("best");
		level = PlayerPrefs.GetInt("level");

		levelHardnessMultiplier = (int)Mathf.Pow(Mathf.Log(level), 2);
		GameManager.Instance.levelMilestone = 30 + levelHardnessMultiplier / 2;
	}

	public static void IncrementScore()
	{
		score += level * multiplier;
	}

	public static void IncrementMultiplier()
	{
		multiplier += 1;
	}

	public static void ResetMultiplier()
	{
		multiplier = 1;
	}

	public static void IncrementLevel()
	{
		level += 1;
		platformsHopped = 0;
		
		PlayerPrefs.SetInt("level",level);
		PlayerPrefs.Save();
		
		levelHardnessMultiplier = (int)Mathf.Pow(Mathf.Log(level), 2);
		GameManager.Instance.levelMilestone = 30 + levelHardnessMultiplier / 2;
	}

	public static void IncrementPlatformsHopped()
	{
		platformsHopped++;
	}
}
