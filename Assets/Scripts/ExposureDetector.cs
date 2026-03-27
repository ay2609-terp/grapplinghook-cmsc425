using UnityEngine;
using System;

public class ExposureDetector : MonoBehaviour
{
    public GameObject LightList;
    public Vector3 OriginOffset;
    public LayerMask OccluderLayers;
    public bool ShowDebugLines;

    public static event Action onEnterShadow;
    public static event Action onEnterLight;
    bool wasLit = false;

    void Start()
    {

    }

    void Update()
    {
        bool lit = false;
        Vector3 origin = transform.position + OriginOffset;

        foreach (Transform child in LightList.transform)
        {
            Light light = child.GetComponent<Light>();
            if (light == null)
            {
                Debug.Log("child of lightList has no light component");
                continue;
            }

            if (light.type == LightType.Spot)
            {
                lit |= inSpotLight(light, origin, ShowDebugLines);
            }
            else if (light.type == LightType.Directional)
            {
                lit |= inDirectionalLight(light, origin, ShowDebugLines);
            }
            else if (light.type == LightType.Point)
            {
                lit |= inPointLight(light, origin, ShowDebugLines);
            }
            else
            {
                Debug.Log(light.type + " light type not handled");
            }
        }

        // send relevant events and set state variable
        if (!lit && wasLit) 
        {
            onEnterShadow?.Invoke();
            Debug.Log("onEnterShadow event invoked");

            wasLit = false;
        }
        else if (lit)
        {
            if (!wasLit)
            {
                onEnterLight?.Invoke();
                Debug.Log("onEnterLight event invoked");
            }

            wasLit = true;
        }
    }

    private bool inSpotLight(Light light, Vector3 position, bool drawDebugLines = false)
    {
        Vector3 lightPosition = light.transform.position;

        // check if position is in the spotlight cone
        float relativeDot = Vector3.Dot(light.transform.forward, (position - lightPosition).normalized);
        relativeDot = Mathf.Clamp(relativeDot, -1f, 1f); // for floating point imprecision 
        float angleThreshold = Mathf.Cos(light.spotAngle * 0.5f * Mathf.Deg2Rad);
        if (relativeDot < angleThreshold)
        {
            if (drawDebugLines)
                Debug.DrawLine(position, lightPosition, Color.yellow);
            return false;
        }

        // check for occlusion with raycast
        Vector3 direction = (lightPosition - position).normalized;
        float distance = Vector3.Distance(position, lightPosition);
        if (Physics.Raycast(position, direction, out RaycastHit hit, distance, OccluderLayers))
        {
            if (drawDebugLines)
                Debug.DrawLine(position, hit.point, Color.red);
            return false;
        }
        
        if (drawDebugLines)
            Debug.DrawLine(position, lightPosition, Color.green);
        return true;
    }

    // stub
    private bool inDirectionalLight(Light light, Vector3 position, bool drawDebugLines = false)
    {
        Debug.Log("ExposureDetector inDirectionalLight Unimplemented");
        return false;
    }

    // stub
    private bool inPointLight(Light light, Vector3 position, bool drawDebugLines = false)
    {
        Debug.Log("ExposureDetector inPointLight Unimplemented");
        return false;
    }
}
