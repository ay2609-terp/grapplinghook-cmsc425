using UnityEngine;
using System.Collections;


public class Switchable : MonoBehaviour, IInteractable
{
    public LightOnOff[] ConnectedLights;

    public Material OnMaterial;
    public Material OffMaterial;
    public Renderer IndicatorLight;
    public GameObject Lever;
    public AudioSource SwitchAudio;

    public float LeverMaxAngle = 60;
    public float RotateSpeed = 8f;
    
    private bool switchEnabled;
    private Coroutine rotateRoutine;

    void Start()
    {
        foreach (LightOnOff light in ConnectedLights)
        {
            light.DisableLight();
        }
    }

    public string ActionTooltip()
    {
        return "[LMB] Switch";
    }

    public void Interact(GameObject player)
    {
        switchEnabled = !switchEnabled;

        if (switchEnabled)
        {
            if (rotateRoutine != null) StopCoroutine(rotateRoutine);
            rotateRoutine = StartCoroutine(SwitchOn());
            foreach (LightOnOff light in ConnectedLights)
            {
                light.EnableLight();
            }
        }
        else
        {
            if (rotateRoutine != null) StopCoroutine(rotateRoutine);
            rotateRoutine = StartCoroutine(SwitchOff());
            foreach (LightOnOff light in ConnectedLights)
            {
                light.DisableLight();
            }
        }

        SwitchAudio.PlayOneShot(SwitchAudio.clip);
    }

    public void InteractRelease(GameObject player)
    {

    }

    IEnumerator SwitchOn()
    {
        IndicatorLight.material = OnMaterial;

        Quaternion target = Quaternion.Euler(LeverMaxAngle, 0f, 0f);

        while (Quaternion.Angle(transform.localRotation, target) > 0.1f)
        {
            Lever.transform.localRotation = Quaternion.Slerp(
                Lever.transform.localRotation,
                target,
                Time.deltaTime * RotateSpeed
            );
            yield return null;
        }

        Lever.transform.localRotation = target;
    }

    IEnumerator SwitchOff()
    {
        IndicatorLight.material = OffMaterial;

        Quaternion target = Quaternion.Euler(-LeverMaxAngle, 0f, 0f);

        while (Quaternion.Angle(transform.localRotation, target) > 0.1f)
        {
            Lever.transform.localRotation = Quaternion.Slerp(
                Lever.transform.localRotation,
                target,
                Time.deltaTime * RotateSpeed
            );
            yield return null;
        }

        Lever.transform.localRotation = target;
    }
}
