using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raptileController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private void Update()
    {
        Movement();
    }
    private void Movement()
    {
        transform.position -= transform.forward * Time.deltaTime * movementSpeed;
    }
}
