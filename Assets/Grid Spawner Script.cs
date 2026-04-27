using System;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                GameObject clonedGridObject = Instantiate(gridObject, new Vector3(x * gridScale, Mathf.FloorToInt(MathF.Pow(UnityEngine.Random.value, 3f) * maxGridObjectHeight) * gridScale, z * gridScale) + position, quaternion.identity);
                Renderer clonedRenderer = clonedGridObject.GetComponent<Renderer>();
                clonedRenderer.material.color = Color.HSVToRGB(0, 0, UnityEngine.Random.Range(800f, 1000f) / 1000f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
