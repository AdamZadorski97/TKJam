using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<raptileController>())
        {
            other.GetComponent<raptileController>().canMove = false;
        }
    }
}
