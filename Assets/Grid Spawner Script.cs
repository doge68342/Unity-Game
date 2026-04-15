using Unity.Mathematics;
using UnityEngine;

public class GridSpawnerScript : MonoBehaviour
{
    public GameObject gridObject;
    public int gridSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Instantiate(gridObject, new Vector3(x * 2, 0, z * 2), quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
