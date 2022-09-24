using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharacterTrigger : MonoBehaviour
{
    public CharacterController characterController;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TriggerLine>())
        {
            characterController.OnOTriggerEnterEEvent.Invoke();
            characterController.isInSpecialActionTriggerE = true;
            characterController.triggerLine = other.GetComponent<TriggerLine>();

        }






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
        if (other.GetComponent<TriggerBanana>())
        {
            other.transform.DOScale(Vector3.zero, 0.2f);
        }
    }
}
