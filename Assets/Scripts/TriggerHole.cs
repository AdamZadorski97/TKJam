using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TriggerHole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<raptileController>())
        {
            other.transform.DOMoveY(-10, 4).OnComplete(()=>Destroy(this.gameObject));
        }
    }
}
