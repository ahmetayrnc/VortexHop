using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
	public TextMeshProUGUI gameOver;
	public TextMeshProUGUI score;
	public TextMeshProUGUI best;

	private void Update()
	{
		score.text = PlayerStats.score + "";
		gameOver.text = "%" + (int)( 100f * PlayerStats.platformsHopped / GameManager.Instance.levelMilestone) + " COMPLETED";
	}

	void Start()
	{
		best.gameObject.SetActive(false);
	}
	
	public void ShowNewBestScore()
	{
		best.text = "New Best: " + PlayerStats.score;
		best.gameObject.SetActive(true);
	}

	public void Restart()
	{
		GameManager.Instance.RestartGame();
	}
}
