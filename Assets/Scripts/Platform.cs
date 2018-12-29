using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
	public bool canMove;

	public Transform perfectEffect;
	public Transform perfect;
	public bool hasPerfect;
	public Transform score;
	public TextMeshProUGUI scoreText;

	public Material platformMaterial;
	public Material tempPlatformMaterial;
	
	public Color defaultColor;
	public Color perfectColor;

	public MeshRenderer platformRenderer;

	public Transform[] platformSizes;
	
	public void SetPerfect()
	{
		hasPerfect = true;
		perfect.gameObject.SetActive(true);
	}

	public void ResetPerfect()
	{
		hasPerfect = false;
		perfect.gameObject.SetActive(false);
		perfectEffect.gameObject.SetActive(false);
	}

	public void ShowScore( int _score )
	{
		scoreText.text = "+" + _score;
		score.gameObject.SetActive(true);
	}

	public void HideScore()
	{
		score.gameObject.SetActive(false);
	}

	public void PerfectColor()
	{
		tempPlatformMaterial.color = defaultColor;
		platformRenderer.material = tempPlatformMaterial;
		tempPlatformMaterial.DOColor(perfectColor, 0.125f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
			{
				platformRenderer.material = platformMaterial;
			});
	}

	public void SetMoveable()
	{
		canMove = true;

		Vector3 newRotation = transform.rotation.eulerAngles;
		newRotation.z += 30f + PlayerStats.levelHardnessMultiplier / 2f;
		transform.DORotate(newRotation, 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
	}

	public void ResetMoveable()
	{
		canMove = false;
		transform.DOKill();
	}

	public void ResetEverything()
	{
		HideScore();
		ResetMoveable();
		ResetPerfect();
		ResetSize();
	}

	public void RandomSize()
	{
		int randNumber = Random.Range(0, platformSizes.Length);
		
		if (PlayerStats.level >= 100f)
		{
			randNumber = Random.Range(3, platformSizes.Length);
		}
		
		if (PlayerStats.level < 50f)
		{
			randNumber = Random.Range(3,4);
		}
		
		if (PlayerStats.level < 20f)
		{
			randNumber = Random.Range(2,3);
		}
		if (PlayerStats.level < 10f)
		{
			randNumber = Random.Range(1,2);
		
		}
		
		if (PlayerStats.level < 5f)
		{
			randNumber = Random.Range(0, 1);
		}
		
		platformSizes[3].gameObject.SetActive(false);
		
		platformSizes[randNumber].gameObject.SetActive(true);

	}

	public void ResetSize()
	{
		for (int i = 0; i < platformSizes.Length; i++)
		{
			platformSizes[i].gameObject.SetActive(false);
		}
		
		platformSizes[3].gameObject.SetActive(true);
	}

	public void PlayPerfectEffect()
	{
		perfectEffect.gameObject.SetActive(true);
	}
}
