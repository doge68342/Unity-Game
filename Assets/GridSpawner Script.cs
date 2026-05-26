using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridSpawnerScript : MonoBehaviour
{
    public GameObject gridObject;
    public int gridSize;
    public Vector3 position;
    public int gridScale;
    public int maxGridObjectHeight;
    public int girdObjectScale;
    public float randomizeInterval;
    public float randomizeTimer;
    private List<GameObject> pillars = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomizeTimer =  randomizeInterval;
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                GameObject clonedGridObject = Instantiate(gridObject, new Vector3(x * gridScale, Mathf.FloorToInt(MathF.Pow(UnityEngine.Random.value, 3f) * maxGridObjectHeight) * gridScale, z * gridScale) + position, quaternion.identity);
                Renderer clonedRenderer = clonedGridObject.GetComponent<Renderer>();
                clonedRenderer.material.color = Color.HSVToRGB(0, 0, UnityEngine.Random.Range(800f, 1000f) / 1000f);
                pillars.Add(clonedGridObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        randomizeTimer -= Time.deltaTime;
        if (randomizeTimer <= 0)
        {
            randomizeTimer = randomizeInterval;
            foreach (GameObject pillar in pillars)
            {
                pillar.transform.position = new Vector3(pillar.transform.position.x, Mathf.FloorToInt(MathF.Pow(UnityEngine.Random.value, 3f) * maxGridObjectHeight) * gridScale + position.y, pillar.transform.position.z);
            }
        }
    }
}
