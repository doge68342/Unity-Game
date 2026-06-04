using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class JumpPadScript : MonoBehaviour
{
    public float power;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            hit.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up *  power, ForceMode.Impulse);
        }
    }
}
