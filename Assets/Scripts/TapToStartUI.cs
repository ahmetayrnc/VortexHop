using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  TMPro;

public class TapToStartUI : MonoBehaviour
{
	public TextMeshProUGUI best;
	public TextMeshProUGUI level;
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			StartGame();
		}

		level.text = "Level: " + PlayerStats.level;
		best.text = "BEST: " + PlayerStats.best + "";
	}
	
	public void StartGame()
	{
		GameManager.Instance.StartGame();
	}
}
