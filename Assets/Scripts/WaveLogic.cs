using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class WaveLogic : MonoBehaviour
{
    public int wave;
    public GameObject player;
    public GameObject drone;
    public int dronesLeft;
    public float spawnDistanceSquare;
    public TMP_Text waveText;
    public TMP_Text dronesLeftText;
    public TMP_Text bestWaveText;
    public int bestWave;
    public List<GameObject> clonedDrones = new List<GameObject>();

    public void Reset()
    {
        for (int i = 0; i < clonedDrones.Count; i++)
        {
            Destroy(clonedDrones[i]);
            dronesLeft = 0;
            wave = 0;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dronesLeft == 0)
        {
            wave += 1;
            dronesLeft = wave;
            for (int i = 0; i < wave; i++)
            {
                GameObject clonedDrone = Instantiate(drone, new Vector3((UnityEngine.Random.value - 0.5f) * 2 * spawnDistanceSquare, 5, (UnityEngine.Random.value - 0.5f) * 2 * spawnDistanceSquare), quaternion.identity);
                clonedDrones.Add(clonedDrone);
                clonedDrone.GetComponent<DroneScript>().target = player;
                clonedDrone.GetComponent<DroneScript>().waveLogic = this;
            }
        }
        waveText.text = "Wave " + wave;
        dronesLeftText.text = dronesLeft + " Drones Left";
        if (wave > bestWave)
        {
            bestWave = wave;
        }
        bestWaveText.text = "Best Wave: " + bestWave;
    }
}
