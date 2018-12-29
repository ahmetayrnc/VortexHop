using UnityEngine;

public class MyAwesomeScript : MonoBehaviour {
    public Camera cam;
    public CameraFollow camFollow;

    public GameObject canvasToOpen;
    public GameObject[] objectsToClose;

    public void OpenVideoMode() {
        canvasToOpen.SetActive(true);
        for (int i = 0; i < objectsToClose.Length; i++) {
            objectsToClose[i].SetActive(false);
        }
    }

    public void StartLikeSecondChance() {
        canvasToOpen.SetActive(false);
        for (int i = 0; i < objectsToClose.Length; i++) {
            objectsToClose[i].SetActive(true);
        }

        GameManager.Instance.SecondChanceWithoutAd();
    }

    private Color defaultColor;
    private void Awake() {
        defaultColor = cam.backgroundColor;
    }

    public void ToggleColor() {
        if (cam.backgroundColor == defaultColor) {
            cam.backgroundColor = Color.black;
        } else {
            cam.backgroundColor = defaultColor;
        }
    }

    public void Forward() {
        camFollow.offsetZ += 1f;
    }

    public void Back() {
        camFollow.offsetZ -= 1f;
    }

    public void Left() {
        camFollow.fixedX -= 1f;
    }

    public void Right() {
        camFollow.fixedX += 1f;
    }

    public void Up() {
        camFollow.fixedY += 1f;
    }

    public void Down() {
        camFollow.fixedY -= 1f;
    }

    public void RotateRight() {
        cam.transform.rotation *= Quaternion.Euler(0f, 5f, 0f);
    }

    public void RotateLeft() {
        cam.transform.rotation *= Quaternion.Euler(0f, -5f, 0f);
    }

    public void RotateUp() {
        cam.transform.rotation *= Quaternion.Euler(5f, 0f, 0f);
    }

    public void RotateDown() {
        cam.transform.rotation *= Quaternion.Euler(-5f, 0f, 0f);
    }
}