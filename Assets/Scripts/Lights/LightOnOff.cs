using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    public LightOnOff lightTarget;

    public Renderer lampModel;

    public Material OnMaterial;
    public Material OffMaterial;

    private Light lightObject;

    void Awake()
    {
        lightObject = gameObject.GetComponentInChildren<Light>();
    }

    public void EnableLight()
    {
        lightObject.enabled = true;
        lampModel.material = OnMaterial;
    }

    public void DisableLight()
    {
        lightObject.enabled = false;
        lampModel.material = OffMaterial;
    }
}
