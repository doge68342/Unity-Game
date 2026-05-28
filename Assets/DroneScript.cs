using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    public GameObject target;
    private Transform targetTransform;
    public float droneSpeed;
    private Rigidbody rb;
    private Renderer[] renderers;

    public float preferedDistance;
    public float stopZone;
    public float fireRate;
    private float shootTimer;
    public GameObject bullet;
    public bool hasLineOfSiteToTarget;
    private int gunSequence = 1;
    public float damage;
    public float maxHealth;
    public float health;
    public float regenerationPercentPerSecond;
    public Color baseColor;
    public Color damagedColor;
    public WaveLogic waveLogic;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        shootTimer = 1/fireRate;
        health = maxHealth;
        renderers = GetComponentsInChildren<Renderer>();
        baseColor = renderers[1].material.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetTransform.position + new Vector3(0, 1, 0)); // tracks ur head 
        shootTimer = math.max(0, shootTimer - Time.deltaTime);
        if (shootTimer <= 0 && hasLineOfSiteToTarget)
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
            BulletScript.damage = damage;

        }



        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, ~LayerMask.GetMask("Bullet")))
        {
            if (hitInfo.collider.gameObject == target)
            {
                hasLineOfSiteToTarget = true;
            }
            else
            {
                hasLineOfSiteToTarget = false;
            }
        }

        if (health <= 0)
        {
            waveLogic.dronesLeft--;
            Destroy(gameObject);
        }

        health = math.min(maxHealth, health + maxHealth * regenerationPercentPerSecond * Time.deltaTime / 100);

        for (int i = 0; i < renderers.Length; i++)
        {
            
            renderers[i].material.color = Color.Lerp(baseColor, damagedColor, 1 - health/maxHealth);   
            
        }

        if (transform.position.y < 5)
        {
            transform.position += new Vector3(0, 50, 0);
        }
    }

    void FixedUpdate()
    {
        float distantToTarget = (targetTransform.position - transform.position).magnitude;
        if (distantToTarget > preferedDistance + stopZone)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, transform.forward * droneSpeed, 1f - Mathf.Exp(-2f * Time.fixedDeltaTime));
        }
        else if (distantToTarget < preferedDistance - stopZone && hasLineOfSiteToTarget)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, -transform.forward * droneSpeed, 1f - Mathf.Exp(-2f * Time.fixedDeltaTime));
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, 1f - Mathf.Exp(-2f * Time.fixedDeltaTime));
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, ~LayerMask.GetMask("Bullet", "Drone")))
        {
            if (hitInfo.distance <= 5 && hitInfo.collider.gameObject != target)
            {
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.up * droneSpeed, 1f - Mathf.Exp(-2f * Time.fixedDeltaTime));
            }
        }
    }
}
