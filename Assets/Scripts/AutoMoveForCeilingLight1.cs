using UnityEngine;

// making the platorm move back and forth
public class AutoMoveForCeilingLight1 : MonoBehaviour
{
    // change distance to liking
    public float max = 3f;
    public float min;
    void Start()
    {
        min = transform.position.x;
        max = transform.position.x + 3;
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.PingPong(Time.time * 2, max - min) + min, transform.position.y, transform.position.z);
    }
}
