using UnityEngine;

public class DeathCounter : MonoBehaviour {
    public static int counter = 0;

    private static DeathCounter Instance;

    private void Awake() {
        if (Instance) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}