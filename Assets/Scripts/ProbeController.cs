using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ProbeController : MonoBehaviour
{    
    public float speed;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 translation = Vector3.zero;

        // lateral translation
        if (Keyboard.current.wKey.isPressed)
        {
            translation += new Vector3(0, 0, speed * Time.deltaTime);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            translation += new Vector3(0, 0, -speed * Time.deltaTime);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            translation += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            translation += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        // vertical translation
        if (Keyboard.current.qKey.isPressed)
        {
            translation += new Vector3(0, -speed * Time.deltaTime, 0);
        }
        if (Keyboard.current.eKey.isPressed)
        {
            translation += new Vector3(0, speed * Time.deltaTime, 0);
        }

        transform.position += translation;
    }
}
