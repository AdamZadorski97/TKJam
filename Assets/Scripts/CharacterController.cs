using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private UnityEvent APressed;
    [SerializeField] private UnityEvent DPressed;
    [SerializeField] private UnityEvent QPressed;
    [SerializeField] private UnityEvent EPressed;
    [SerializeField] private UnityEvent SpacePressed;
    [SerializeField] private UnityEvent OnObstacleEnterEvent;
    [SerializeField] private UnityEvent OnOTriggerEnterQEvent;
    [SerializeField] private UnityEvent OnOTriggerEnterEEvent;
    [SerializeField] private UnityEvent OnOTriggerExitQEvent;
    [SerializeField] private UnityEvent OnOTriggerExitEEvent;
    [SerializeField] private float movementSpeed;

    [SerializeField] private Transform playerCharacter;
    [SerializeField] private float leftPosition;
    [SerializeField] private float middlePosition;
    [SerializeField] private float rightPosition;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpHigh;
    [SerializeField] private float moveTime;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private int currentTrack;
    private Sequence moveSequence;

    [SerializeField] private AnimationCurve jumpUpCurve;
    [SerializeField] private AnimationCurve jumpDownCurve;
    private Sequence jumpSequence;

    private bool isInSpecialActionTriggerE;
    private bool isInSpecialActionTriggerQ;
    private UnityAction specialAction;
    private void Movement()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.A))
        {
            APressed.Invoke();
            OnMoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DPressed.Invoke();
            OnMoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QPressed.Invoke();
            OnQPressed();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EPressed.Invoke();
            OnEPressed();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpacePressed.Invoke();
            OnSpacePressed();
        }
    }

    private void OnMoveLeft()
    {
        if (currentTrack == 0) return;

        if (currentTrack == 1)
        {
            if (moveSequence != null)
            {
                moveSequence.Kill();
            }
            moveSequence = DOTween.Sequence();
            moveSequence.Append(playerCharacter.DOLocalMoveX(leftPosition, moveTime).SetEase(moveCurve)).OnComplete(() => moveSequence.Kill());
            currentTrack = 0;
            return;
        }

        if (currentTrack == 2)
        {
            if (moveSequence != null)
            {
                moveSequence.Kill();
            }
            moveSequence = DOTween.Sequence();
            moveSequence.Append(playerCharacter.DOLocalMoveX(middlePosition, moveTime).SetEase(moveCurve)).OnComplete(() => moveSequence.Kill());
            currentTrack = 1;
            return;
        }
    }

    private void OnMoveRight()
    {
        if (currentTrack == 0)
        {
            if (moveSequence != null)
            {
                moveSequence.Kill();
            }
            moveSequence = DOTween.Sequence();
            moveSequence.Append(playerCharacter.DOLocalMoveX(middlePosition, moveTime).SetEase(moveCurve)).OnComplete(() => moveSequence.Kill());
            currentTrack = 1;
            return;
        }


        if (currentTrack == 1)
        {
            if (moveSequence != null)
            {
                moveSequence.Kill();
            }
            moveSequence = DOTween.Sequence();
            moveSequence.Append(playerCharacter.DOLocalMoveX(rightPosition, moveTime).SetEase(moveCurve)).OnComplete(() => moveSequence.Kill());
            currentTrack = 2;
            return;
        }

        if (currentTrack == 2) return;
    }

    private void OnQPressed()
    {
        if (isInSpecialActionTriggerQ)
        {
            specialAction.Invoke();
        }

    }

    private void OnEPressed()
    {
        if (isInSpecialActionTriggerE)
        {
            specialAction.Invoke();
        }
    }

    private void OnSpacePressed()
    {
        if (jumpSequence != null)
        {
            jumpSequence.Kill();
        }
            jumpSequence = DOTween.Sequence();
            jumpSequence.Append(playerCharacter.DOLocalMoveY(jumpHigh, jumpTime).SetEase(jumpUpCurve));
            jumpSequence.Append(playerCharacter.DOLocalMoveY(0, jumpTime).SetEase(jumpDownCurve));
        
    }

        private void OnObstacleEnter()
        {
            OnObstacleEnterEvent.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<TriggerAction>())
            {
                if (other.GetComponent<TriggerAction>().actionType == "E")
                {
                    OnOTriggerEnterEEvent.Invoke();
                    isInSpecialActionTriggerE = true;
                }

                if (other.GetComponent<TriggerAction>().actionType == "Q")
                {
                    OnOTriggerEnterQEvent.Invoke();
                    isInSpecialActionTriggerQ = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<TriggerAction>())
            {
                if (other.GetComponent<TriggerAction>().actionType == "E")
                {
                    isInSpecialActionTriggerE = false;
                    OnOTriggerExitEEvent.Invoke();
                }

                if (other.GetComponent<TriggerAction>().actionType == "Q")
                {
                    isInSpecialActionTriggerQ = false;
                    OnOTriggerExitQEvent.Invoke();
                }
            }

            if (other.GetComponent<TriggerObstacle>())
            {
                OnObstacleEnter();
            }
        }
    }
