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
    private int[] targetPillarHeights;
    private int[] currentPillarHeights;
    public AnimationCurve movementCurve;
    public float animationLength;
    public float animationTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomizeTimer = 0;
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                GameObject clonedGridObject = Instantiate(gridObject, new Vector3(x * gridScale, 0, z * gridScale) + position, quaternion.identity);
                Renderer clonedRenderer = clonedGridObject.GetComponent<Renderer>();
                clonedRenderer.material.color = Color.HSVToRGB(0, 0, UnityEngine.Random.Range(800f, 1000f) / 1000f);
                pillars.Add(clonedGridObject);
            }
        }
        targetPillarHeights = new int[pillars.Count];
        currentPillarHeights = new int[pillars.Count];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        randomizeTimer -= Time.fixedDeltaTime;
        animationTimer -= Time.fixedDeltaTime;
        if (randomizeTimer <= 0)
        {
            randomizeTimer = randomizeInterval;
            animationTimer = animationLength;
            for (int  i = 0; i < pillars.Count; i++)
            {
                currentPillarHeights[i] = Mathf.FloorToInt(pillars[i].transform.position.y);
                targetPillarHeights[i] = Mathf.FloorToInt(MathF.Pow(UnityEngine.Random.value, 3f) * maxGridObjectHeight);
            }
        }
        for (int i = 0; i < pillars.Count; i++)
        {
            // pillars[i].GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(pillars[i].transform.position, new Vector3(pillars[i].transform.position.x, targetPillarHeights[i] * gridScale + position.y, pillars[i].transform.position.z), 1f - Mathf.Exp(-0.5f * Time.fixedDeltaTime)));
            pillars[i].GetComponent<Rigidbody>().MovePosition(new Vector3(pillars[i].transform.position.x, Mathf.Lerp(currentPillarHeights[i], targetPillarHeights[i] * gridScale + position.y, movementCurve.Evaluate(1 - (animationTimer / animationLength))), pillars[i].transform.position.z));
        }
    }
}
