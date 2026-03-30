using UnityEngine;

public class FanSpin : MonoBehaviour
{
    public float speed = 200f;

    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}