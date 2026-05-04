using UnityEngine;

public class EnableUI : MonoBehaviour
{
    public Canvas Canvas;

    void Awake()
    {
        Canvas.gameObject.SetActive(true);
    }
}
