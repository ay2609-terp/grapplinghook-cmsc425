using UnityEngine;

public class MusicManagerBackground : MonoBehaviour
{
    private static MusicManagerBackground instance;

    void Awake()
    {
        // if alr exist, remove new instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // continue throughout scenes
        DontDestroyOnLoad(gameObject);
    }
}