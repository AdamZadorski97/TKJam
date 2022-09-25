using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raptileController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private CharacterController characterController;
    public bool canMove;
    private void Awake()
    {
        characterController = FindObjectOfType<CharacterController>();
    }

    private void Update()
    {
   
        if (characterController != null)
        {
            if(Vector3.Distance(characterController.transform.position, transform.position) < 15)
            if(canMove)
                Movement();
        }

    }
    private void Movement()
    {
        transform.position -= transform.forward * Time.deltaTime * movementSpeed;
    }
}
