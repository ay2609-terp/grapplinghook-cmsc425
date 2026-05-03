using UnityEngine;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    private bool isOpen = false;

    public void ToggleDoor()
    {
        if (!isOpen)
        {
            transform.Rotate(0, openAngle, 0);
        }
        else
        {
            transform.Rotate(0, -openAngle, 0);
        }

        isOpen = !isOpen;
    }
}