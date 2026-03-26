using UnityEngine;
using System;

public class ExposureDetector : MonoBehaviour
{
    public GameObject LightList;
    public Vector3 OriginOffset;
    public LayerMask OccluderLayers;

    public static event Action onEnterShadow;
    bool wasLit = false;

    void Start()
    {

    }

    void Update()
    {
        bool lit = false;
        Vector3 origin = transform.position += OriginOffset;

        foreach (Transform child in LightList.transform)
        {
            Light light = child.GetComponent<Light>();

            // note: if other factors need to be considered refactor if/else structure
            if (light != null)
            {
                Vector3 target = child.transform.position;
                Vector3 direction = (target - origin).normalized;
                float distance = Vector3.Distance(origin, target);
                
                // no direct line of sight to light
                if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, OccluderLayers))
                {
                    Debug.DrawLine(origin, hit.point, Color.red);
                }
                // direct line of sight to light; check angles
                else
                {
                    float relativeDot = Vector3.Dot(light.transform.forward, (origin - target).normalized);
                    relativeDot = Mathf.Clamp(relativeDot, -1f, 1f); // for floating point imprecision 

                    float angleThreshold = Mathf.Cos(light.spotAngle * 0.5f * Mathf.Deg2Rad);

                    if (relativeDot >= angleThreshold)
                    {
                        Debug.DrawLine(origin, target, Color.green);
                        lit = true;
                    }
                    else
                    {
                        Debug.DrawLine(origin, target, Color.yellow);
                    }
                }
            }
            else
            {
                Debug.Log("child of lightList has no light component");
            }
        }

        if (!lit && wasLit) 
        {
            onEnterShadow?.Invoke();
            Debug.Log("onEnterShadow event invoked");

            wasLit = false;
        }
        else if (lit)
        {
            wasLit = true;
        }
    }
}
