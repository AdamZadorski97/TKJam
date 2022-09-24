using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrigger : MonoBehaviour
{
    public CharacterController characterController;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TriggerAction>())
        {
            if (other.GetComponent<TriggerAction>().actionType == "E")
            {
                characterController.OnOTriggerEnterEEvent.Invoke();
                characterController.isInSpecialActionTriggerE = true;
            }

            if (other.GetComponent<TriggerAction>().actionType == "Q")
            {
                characterController.OnOTriggerEnterQEvent.Invoke();
                characterController.isInSpecialActionTriggerQ = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TriggerAction>())
        {
            if (other.GetComponent<TriggerAction>().actionType == "E")
            {
                characterController.isInSpecialActionTriggerE = false;
                characterController.OnOTriggerExitEEvent.Invoke();
            }

            if (other.GetComponent<TriggerAction>().actionType == "Q")
            {
                characterController.isInSpecialActionTriggerQ = false;
                characterController.OnOTriggerExitQEvent.Invoke();
            }
        }

        if (other.GetComponent<TriggerObstacle>())
        {
            characterController.OnObstacleEnter();

        }
    }
}