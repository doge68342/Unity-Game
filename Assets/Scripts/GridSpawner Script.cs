using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Xml;
using TMPro;
//using TMPro.EditorUtilities;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
    private List<GameObject> bouncePads = new List<GameObject>();
    public int bouncePadsCount;
    public int shortCount;
    private int[] targetPillarHeights;
    private int[] currentPillarHeights;
    public AnimationCurve movementCurve;
    public float animationLength;
    public float animationTimer;
    public int bouncePadChance;
    public GameObject bouncePad;
    public Image refreshBar;
    public Vector2 originalRefreshBarSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalRefreshBarSize = refreshBar.rectTransform.rect.size;
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
        bouncePadsCount = bouncePads.Count;
        refreshBar.rectTransform.sizeDelta = new Vector2(randomizeTimer / randomizeInterval * originalRefreshBarSize.x, originalRefreshBarSize.y);
    }

    void FixedUpdate()
    {
        randomizeTimer -= Time.fixedDeltaTime;
        animationTimer -= Time.fixedDeltaTime;
        if (randomizeTimer <= 0)
        {
            randomizeTimer = randomizeInterval;
            animationTimer = animationLength;
            for (int i = 0; i < pillars.Count; i++)
            {
                currentPillarHeights[i] = Mathf.FloorToInt(pillars[i].transform.position.y);
                targetPillarHeights[i] = Mathf.FloorToInt(MathF.Pow(UnityEngine.Random.value, 3f) * maxGridObjectHeight);
            }
        }

        if (animationTimer <= 0 && bouncePads.Count == 0)
        {
            shortCount = 0;
            for (int i = 0; i < pillars.Count; i++)
            {

                if (Mathf.Approximately(pillars[i].transform.position.y, position.y)) shortCount++;
                if (Mathf.Approximately(pillars[i].transform.position.y, position.y) && UnityEngine.Random.value * bouncePadChance <= 1f)
                {
                    GameObject clonedBouncePad = Instantiate(bouncePad);
                    bouncePads.Add(clonedBouncePad);
                    clonedBouncePad.transform.position = pillars[i].transform.position + new Vector3(0, 0.15f, 0);
                }
            }
        }

        if (animationTimer > 0)
        {
            for (int i = 0; i < bouncePads.Count; i++)
            {
                Destroy(bouncePads[i]);
            }
            bouncePads.Clear();
        }

        for (int i = 0; i < pillars.Count; i++)
        {
            pillars[i].GetComponent<Rigidbody>().MovePosition(new Vector3(pillars[i].transform.position.x, Mathf.Lerp(currentPillarHeights[i], targetPillarHeights[i] * gridScale + position.y, movementCurve.Evaluate(1 - (animationTimer / animationLength))), pillars[i].transform.position.z));
        }
    }
}
