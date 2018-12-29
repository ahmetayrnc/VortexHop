using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTransitionUI : MonoBehaviour
{

	public TextMeshProUGUI levelTransitionText;

	void Update()
	{
		levelTransitionText.text = "Vortex " + PlayerStats.level;
	}
}
