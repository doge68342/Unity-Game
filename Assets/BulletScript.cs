using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletSpeed = 10;
    public GameObject target;
    public GameObject gun;
    private Renderer renderer;
    public float lifeTime;
    private float lifeTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        transform.LookAt(target.transform.position + new Vector3(0, 1, 0));
        lifeTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject != gun && hit.gameObject.layer != LayerMask.NameToLayer("Bullet") && hit.gameObject.layer != LayerMask.NameToLayer("Drone"))
        {
           Destroy(gameObject); 
        }
    }
}
