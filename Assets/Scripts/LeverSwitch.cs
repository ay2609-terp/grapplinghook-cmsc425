using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    public Light[] lightsToTurnOff;
    private bool isOn = true;
    // variable to set the intensity of light when on
    public float intensity;

    public void PullLever()
    {
        isOn = !isOn;

        for (int i = 0; i < lightsToTurnOff.Length; i++)
        {
            if (isOn)
            {
                lightsToTurnOff[i].intensity = intensity;
            }
            else
            {
                lightsToTurnOff[i].intensity = 0f;
            }
        }
    }
}
