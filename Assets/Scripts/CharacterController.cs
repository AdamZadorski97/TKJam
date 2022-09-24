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
    [SerializeField] public UnityEvent OnObstacleEnterEvent;
    [SerializeField] public UnityEvent OnOTriggerEnterQEvent;
    [SerializeField] public UnityEvent OnOTriggerEnterEEvent;
    [SerializeField] public UnityEvent OnOTriggerExitQEvent;
    [SerializeField] public UnityEvent OnOTriggerExitEEvent;
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
    private bool isInJumpState;
    [SerializeField] private AnimationCurve jumpUpCurve;
    [SerializeField] private AnimationCurve jumpDownCurve;
    private Sequence jumpSequence;

    public bool isInSpecialActionTriggerE;
    public bool isInSpecialActionTriggerQ;
    private UnityAction specialAction;
    public bool canMove;
    public GameMachineSmallScreenController gameMachineSmallScreenController;

    public int hp;
    private void Movement()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }

    private void Update()
    {
        if (canMove)
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
        if (isInJumpState)
            return;
        if (jumpSequence != null)
        {
            jumpSequence.Kill();
        }
        jumpSequence = DOTween.Sequence();
        jumpSequence.AppendCallback(() => isInJumpState = true);
        jumpSequence.Append(playerCharacter.DOLocalMoveY(jumpHigh, jumpTime).SetEase(jumpUpCurve));
        jumpSequence.Append(playerCharacter.DOLocalMoveY(0.25f, jumpTime).SetEase(jumpDownCurve));
        jumpSequence.AppendCallback(() => isInJumpState = false);


    }

    public void OnObstacleEnter()
    {
        hp--;
        gameMachineSmallScreenController.ShowMessage("", 2, true, true);
        OnObstacleEnterEvent.Invoke();
    }


 
}
