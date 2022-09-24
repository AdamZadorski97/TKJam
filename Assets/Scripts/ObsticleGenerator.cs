using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ObsticleGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tileObsticlesLocalisations;
    [SerializeField] private List<GameObject> avilableTileObsticles;
    [SerializeField] private GameObject obsticle;

    [SerializeField] private Vector2 spawningRate;
    [SerializeField] private Vector2 xSpawnPosition;
    [SerializeField] private Vector2 zSpawnPosition;
    [SerializeField] private float ySpawnPosition;

    private void Start()
    {
        RandomizeObsticle();
    }

    private void RandomizeObsticle()
    {
        for (int i = 0; i < tileObsticlesLocalisations.Count; i++)
        {
           GameObject newObstacle = Instantiate(avilableTileObsticles[Random.Range(0, avilableTileObsticles.Count)]);
           newObstacle.transform.position = tileObsticlesLocalisations[i].transform.position;

           //tileObsticles[i].transform.position = 
        }
        
    }

    private Vector3 RandomPosition()
    {
        float xRandomSpawnPositon = UnityEngine.Random.Range(xSpawnPosition.x, xSpawnPosition.y);
        float zRandomSpawnPositon = UnityEngine.Random.Range(zSpawnPosition.x, zSpawnPosition.y);
        return new Vector3(xRandomSpawnPositon, ySpawnPosition, zRandomSpawnPositon);
    }
}


//odległość tila 20 jednostek 