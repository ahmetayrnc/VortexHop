using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

using GameAnalyticsSDK;
using Facebook.Unity;

using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	
	public TapToStartUI tapToStartUI;
	public ScoreUI scoreUI;
	public GameOverUI gameOverUI;
	public LevelTransitionUI levelTransitionUI;
	public GameObject playerHolder;
	public PlayerMovement playerMovement;

	public GameObject swipeToPlay;

	public int levelMilestone;
	public GameObject secondChanceObject;
	private bool secondChance = false;
	
	void Awake()
	{
		Instance = this;
		Application.targetFrameRate = 60;
	}

	void Start()
	{
		tapToStartUI.gameObject.SetActive(true);
		scoreUI.gameObject.SetActive(false);
		gameOverUI.gameObject.SetActive(false);
		levelTransitionUI.gameObject.SetActive(false);
        tempPlatform.platformMaterial.SetFloat("Vector1_DCC11506", threshold);

		ColorSelection(PlayerPrefs.GetInt("Color", 0));
    }

	void Update()
	{
		if (PlayerStats.platformsHopped >= levelMilestone)
		{
			NextLevel();
		}

		if (swipeToPlay.activeInHierarchy) {
			if (Input.GetMouseButtonDown(0)) {
				swipeToPlay.SetActive(false);
				playerMovement.StartMovement();
			}
		}
	}
	
	public GameObject GetPlayerHolder()
	{
		return playerHolder;
	}

	public void StartGame()
	{
		scoreUI.gameObject.SetActive(true);
		tapToStartUI.gameObject.SetActive(false);
		playerMovement.StartMovement();

		GameAnalytics.NewDesignEvent("Levels:" + PlayerStats.level);
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LEVEL" + PlayerStats.level.ToString("000"));
    }
	
	public void RestartGame()
	{
        tempPlatform.platformMaterial.SetFloat("Vector1_DCC11506", threshold);

		if (DeathCounter.counter > 3) {
			AdsManager.ShowInterstitial();
		} else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}
	
	public void EndGame()
	{
		scoreUI.gameObject.SetActive(false);
		gameOverUI.gameObject.SetActive(true);
		
		if (PlayerStats.score > PlayerStats.best)
		{
			gameOverUI.ShowNewBestScore();
			PlayerStats.best = PlayerStats.score;
			PlayerPrefs.SetInt("best", PlayerStats.best);
			PlayerPrefs.Save();
		}
		
		#if (UNITY_ANDROID)
		#else
		iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
		#endif
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", PlayerStats.score);

        DeathCounter.counter++;

		if (!secondChance) {
			secondChanceObject.SetActive(true);
		}
    }

	public void NextLevel()
	{
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LEVEL" + PlayerStats.level.ToString("000"));
        var param = new Dictionary<string, object>();
        param[AppEventParameterName.Level] = "LEVEL" + PlayerStats.level.ToString("000");
        FB.LogAppEvent(AppEventName.AchievedLevel, null, param);

		PlayerStats.IncrementLevel();
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LEVEL" + PlayerStats.level.ToString("000"));
		//levelMilestone += PlayerStats.levelHardnessMultiplier / 2;
		#if (UNITY_ANDROID)
		#else
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
		#endif

		StartCoroutine(LevelTransition());
	}

	IEnumerator LevelTransition()
	{
		Time.timeScale = 0.25f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		levelTransitionUI.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.22f);
		LevelPass.Instance.DestroyLevelPass();
		#if (UNITY_ANDROID)
		#else
			iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
		#endif
        StartCoroutine(ChangePlatformColor());
		yield return new WaitForSeconds(0.25f);
		levelTransitionUI.gameObject.SetActive(false);
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
		yield return new WaitForSeconds(1f);
	}

	IEnumerator ChangePlatformColor() 
	{
		var th = threshold;
        tempPlatform.platformMaterial.SetFloat("Vector1_DCC11506", th);

		previousColor = newColor;
		newColor = colors[Random.Range(0, colors.Count)];
		while (newColor == previousColor) {
			newColor = colors[Random.Range(0, colors.Count)];
		}
		PlayerPrefs.SetInt("Color", colors.FindIndex((x) => x == previousColor));

        while (th < 300f) 
		{
			th += Time.deltaTime * speed;
            tempPlatform.platformMaterial.SetFloat("Vector1_DCC11506", th);

			yield return null;
        }

		tempPlatform.platformMaterial.SetColor("Color_9641F80F", previousColor);
		tempPlatform.platformMaterial.SetColor("Color_C155A686", newColor);
        tempPlatform.platformMaterial.SetFloat("Vector1_DCC11506", threshold);

        LevelPass.Instance.PlaceNewLevelPass();
    }

	private void ColorSelection(int previousIndex) {
		previousColor = colors[previousIndex];
		newColor = colors[Random.Range(0, colors.Count)];
		while (newColor == previousColor) {
			newColor = colors[Random.Range(0, colors.Count)];
		}

		tempPlatform.platformMaterial.SetColor("Color_9641F80F", previousColor);
		tempPlatform.platformMaterial.SetColor("Color_C155A686", newColor);
		
	}

	public void SecondChance() {
		System.Action reward = () => {
			gameOverUI.gameObject.SetActive(false);
			scoreUI.gameObject.SetActive(true);

			var platforms = PlatformPooler.Instance.activePlatforms;
			var min = 99999f;
			var plat = platforms[0];

			for (int i = 0; i < platforms.Count; i++) {
				if (platforms[i].transform.position.z - playerHolder.transform.position.z < min) {
					min = platforms[i].transform.position.z - playerHolder.transform.position.z;
					plat = platforms[i];
				}
			}

			plat.transform.rotation = Quaternion.identity;
			playerHolder.transform.rotation = Quaternion.identity;
			playerHolder.transform.position = plat.transform.position;
			playerMovement.ball.transform.localPosition = playerMovement.jumpBottom.localPosition;

			playerMovement.ball.DOKill();

			swipeToPlay.SetActive(true);
			secondChanceObject.SetActive(false);
			secondChance = true;
		};

		AdsManager.ShowRewarded(reward);
	}

	public void SecondChanceWithoutAd() {
		gameOverUI.gameObject.SetActive(false);
		scoreUI.gameObject.SetActive(true);

		var platforms = PlatformPooler.Instance.activePlatforms;
		var min = 99999f;
		var plat = platforms[0];

		for (int i = 0; i < platforms.Count; i++) {
			if (platforms[i].transform.position.z - playerHolder.transform.position.z < min) {
				min = platforms[i].transform.position.z - playerHolder.transform.position.z;
				plat = platforms[i];
			}
		}

		plat.transform.rotation = Quaternion.identity;
		playerHolder.transform.rotation = Quaternion.identity;
		playerHolder.transform.position = plat.transform.position;
		playerMovement.ball.transform.localPosition = playerMovement.jumpBottom.localPosition;

		playerMovement.ball.DOKill();

		swipeToPlay.SetActive(true);
		secondChanceObject.SetActive(false);
		secondChance = true;
	}

	public float threshold;
	public float speed;
	public Platform tempPlatform;

	[ColorUsageAttribute(true,true)]
	public List<Color> colors;
	
	private Color previousColor;
	public Color newColor;
}
