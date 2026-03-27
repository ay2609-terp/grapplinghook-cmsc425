using UnityEngine;

// making the platorm move back and forth
public class AutoMoveXAxis : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }
}
