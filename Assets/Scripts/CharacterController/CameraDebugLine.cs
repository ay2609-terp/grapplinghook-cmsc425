using UnityEngine;

public class CameraDebugLine : MonoBehaviour
{
    public Transform CameraTransform; 
    
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 cameraPosition = CameraTransform.position;
        Debug.DrawLine(cameraPosition, cameraPosition + (CameraTransform.forward * 2), Color.blue);
    }
}
