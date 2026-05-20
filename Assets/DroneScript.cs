using UnityEngine;

public class DroneScript : MonoBehaviour
{
    public GameObject target;
    private Transform targetTransform;
    public float droneSpeed;
    private Rigidbody rb;
    public float preferedDistance;
    public float stopZone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         transform.LookAt(targetTransform);
    }
    void FixedUpdate()
    {
        // Rotate through physics so it stays in sync with velocity
        float distantToTarget = (targetTransform.position - transform.position).magnitude;
        if (distantToTarget > preferedDistance)
        {
            rb.linearVelocity = transform.forward * droneSpeed;
        }
        else if (distantToTarget < preferedDistance + stopZone)
        {
            rb.linearVelocity = -transform.forward * droneSpeed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
