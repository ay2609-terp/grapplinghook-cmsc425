using UnityEngine;
using System.Collections;

public class LightToggle : MonoBehaviour
{
    public Light lampLight;

    public float interval = 3f;

    void Start()
    {
        if (lampLight == null)
        {
            lampLight = GetComponent<Light>();
        }

        StartCoroutine(ToggleLoop());
    }

    IEnumerator ToggleLoop()
    {
        while (true)
        {
            lampLight.enabled = !lampLight.enabled;
            yield return new WaitForSeconds(interval);
        }
    }
}