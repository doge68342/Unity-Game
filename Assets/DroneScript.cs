using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    public GameObject target;
    private Transform targetTransform;
    public float droneSpeed;
    private Rigidbody rb;
    public float preferedDistance;
    public float stopZone;
    public float fireRate;
    private float shootTimer;
    public GameObject bullet;
    private int gunSequence = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        shootTimer = 1/fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetTransform);
        shootTimer = math.max(0, shootTimer - Time.deltaTime);
        if (shootTimer <= 0)
        {
            shootTimer = 1/fireRate;

            GameObject newBullet = Instantiate(bullet, 
            transform.TransformPoint(new Vector3(5.5f * gunSequence, 0.5f, 7.5f)), 
            quaternion.identity);

            if (gunSequence == 1)
            {
                gunSequence = -1;
            }
            else
            {
                gunSequence = 1;
            }

            BulletScript BulletScript = newBullet.GetComponent<BulletScript>();
            BulletScript.target = target;
            BulletScript.gun = gameObject;
        }
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
