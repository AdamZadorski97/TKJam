using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharacterTrigger : MonoBehaviour
{
    public CharacterController characterController;
    public GameMachineSmallScreenController gameMachineSmallScreenController;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TriggerLine>())
        {
            gameMachineSmallScreenController.ShowMessage("Press E", 1f, true, false);
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
                gameMachineSmallScreenController.ShowMessage("Press Q", 0.5f, true, false);
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
            other.GetComponent<TriggerBanana>().SpawnParticles();
            gameMachineSmallScreenController.ShowMessage("+10pkt", 0.5f, true, false);
            gameMachineSmallScreenController.points += 10;
            other.transform.DOScale(Vector3.zero, 0.2f);
        }
    }
}
