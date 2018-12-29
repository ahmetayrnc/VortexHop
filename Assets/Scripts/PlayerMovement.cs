using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
	public Transform rayHitPoint;
	public Transform jumpTop;
	public Transform jumpBottom;
	public Transform deathPos;
	public Transform ball;
	
	public float speed;
	public float horizontalSpeed;
	public bool doMovement;
	public bool doHorizontalMovement;
	
	private Vector3 firstPos, lastPos;
	private float distanceX;
	private bool isTouched;

	private bool secondStep;
	
	void Start()
	{
		doMovement = true;
		doHorizontalMovement = true;
		secondStep = false;
	}

	public void StartMovement()
	{	
		touchDown();

		doMovement = true;
		Jump();
		transform.DOMoveZ(transform.position.z + 25f, 0.44f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).OnStepComplete(() =>
		{
			var pos = transform.position;
			pos.z = (int) pos.z;
			transform.position = pos;
		});
	}
	
	public void Jump()
	{
		ball.DOLocalMoveY(jumpTop.position.y, 0.22f).SetLoops(-1, LoopType.Yoyo).OnStepComplete(() =>
		{

			if (secondStep)
			{
				//RaycastHit hit;
				//if (Physics.Raycast(rayHitPoint.position, -rayHitPoint.up, out hit, 100f, LayerMask.GetMask("Perfect")))
				//{
				//	Debug.Log("hit Perfect");
				//	
				//	PlayerStats.IncrementMultiplier();
				//}

				// executes if hit a platform
				RaycastHit hit;
				if (Physics.Raycast(rayHitPoint.position, -rayHitPoint.up, out hit, 100f))
				{	
					hit.transform.parent.DOScale(new Vector3(1.5f, 1, 1.5f), 0.1f).SetLoops(2,LoopType.Yoyo);

					Platform currentPlatform = hit.transform.GetComponentInParent<Platform>();
					
					// executes if hit a perfect
					if ( currentPlatform.hasPerfect )
					{
						if (hit.transform.CompareTag("Perfect"))
						{
							PlayerStats.IncrementMultiplier();
							ScoreUI.Instance.CreateMotivationPopUp(PlayerStats.multiplier);
							//currentPlatform.PerfectColor();
							currentPlatform.PlayPerfectEffect();
						}
						else
						{
							PlayerStats.ResetMultiplier();
						}
						#if (UNITY_ANDROID)

						#else
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
						#endif
					}
					
					//ScoreUI.Instance.CreateScorePopUp();
					currentPlatform.ShowScore(PlayerStats.multiplier * PlayerStats.level);
					
					
					PlayerStats.IncrementPlatformsHopped();
					PlayerStats.IncrementScore();
					
					PlatformPooler.Instance.CreateNextPlatform();
					PlatformPooler.Instance.DestroyPlatform(hit.transform.gameObject, 1f);
					
				}
				else
				{
					EndGame();
				}
			}

			secondStep = !secondStep;
		});
	}

	void EndGame()
	{
		DOTween.KillAll();
		ball.DOLocalMoveY(-20f, 1f);
		doMovement = false;
		GameManager.Instance.EndGame();
	}
	
	void Update()
	{
		if (!doMovement)
		{
			return;
		}
		
		if (Input.GetMouseButtonDown(0))
		{
			touchDown();
		}

		if (Input.GetMouseButtonUp(0))
		{
			touchUp();
		}
		
		if (!doHorizontalMovement)
		{
			return;
		}
		
		if (isTouched)
		{
			lastPos = Input.mousePosition;
			lastPos.x -= (float) Screen.width / 2;
			lastPos.x = lastPos.x / Screen.width;
			distanceX = lastPos.x - firstPos.x;
			
			Vector3 ObjLocalPos = transform.localPosition;
				
			if ((ObjLocalPos.x < -7f && distanceX < 0) || (ObjLocalPos.x > 7f && distanceX > 0))
			{
				distanceX = 0;
			}

			Vector3 newRotation = transform.rotation.eulerAngles + Vector3.forward * distanceX * horizontalSpeed;
			
			transform.rotation = Quaternion.Euler(newRotation);
			firstPos = lastPos;
		}
	}
	
	public void touchDown()
	{
		firstPos = Input.mousePosition;
		firstPos.x -= (float)Screen.width / 2;
		firstPos.x = firstPos.x / Screen.width;
		
		isTouched = true;
	}

	public void touchUp()
	{
		isTouched = false;
	}
}
