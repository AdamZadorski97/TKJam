using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

//using UnityEngine.Random

public class TilesController : MonoBehaviour
{
    public GameObject[] tilesPrefabs;
    public float zSpawn;
    public float tileLength = 20f;
    public int tilesAmount = 4;

    public Transform playerTransform;

    private float tempPosition;
    
    private void Start()
    {
        for (int i = 0; i < tilesAmount; i++)
        {
            if (i <= 1)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile(Random.Range(2, tilesPrefabs.Length));
            }
        }
    }

    private void Update()
    {
        if (playerTransform.position.z > zSpawn - (tilesAmount * tileLength))
        {
            SpawnTile(Random.Range(2, tilesPrefabs.Length));
        }
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject chunk = Instantiate(tilesPrefabs[tileIndex]);
        chunk.transform.position = new Vector3(0, 0, tempPosition);
        transform.eulerAngles = new Vector3(-90, 0, 0);
        zSpawn += tileLength;
        tempPosition += 20;
    }
}