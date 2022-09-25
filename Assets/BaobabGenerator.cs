using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaobabGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tileBaobabLocalisationsLeft;
    [SerializeField] private List<GameObject> tileBaobabLocalisationsRight;

    [SerializeField] private List<GameObject> baobabsToSwitchOn;
    
    [SerializeField] private List<GameObject> avilableTileObsticlesLeft;
    [SerializeField] private List<GameObject> avilableTileObsticlesRight;

    [SerializeField] private GameObject bannana;

    private void Start()
    {
        RandomizeTree();
    }

    private void RandomizeTree()
    {
        baobabsToSwitchOn[Random.Range(0, baobabsToSwitchOn.Count)].SetActive(true);
    }
}