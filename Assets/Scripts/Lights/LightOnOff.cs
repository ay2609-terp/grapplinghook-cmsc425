using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    public Renderer lampModel;

    public Material onMaterial;
    public Material offMaterial;

    private Light lightObject;

    void Awake()
    {
        lightObject = gameObject.GetComponentInChildren<Light>();
    }

    public void EnableLight()
    {
        lightObject.enabled = true;
        lampModel.material = onMaterial;
    }

    public void DisableLight()
    {
        lightObject.enabled = false;
        lampModel.material = offMaterial;
    }
}
