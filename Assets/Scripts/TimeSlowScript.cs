using UnityEngine;

public class TimeSlowScript : MonoBehaviour
{
    private float normalFixedDeltaTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        normalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlowTime(float timeSlowFactor)
    {
        Time.timeScale = timeSlowFactor;
        Time.fixedDeltaTime = normalFixedDeltaTime * timeSlowFactor;
    }
}
