using Unity.Mathematics;
using UnityEngine;

public class GridSpawnerScript : MonoBehaviour
{
    public GameObject gridObject;
    public int gridSize;
    public Vector3 position;
    public int gridScale;
    public int maxGridObjectHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Instantiate(gridObject, new Vector3(x * gridScale, UnityEngine.Random.Range(1, maxGridObjectHeight) * 200/215 * 3, z * gridScale) + position, quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
