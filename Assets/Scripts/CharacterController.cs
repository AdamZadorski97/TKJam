using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    public TriggerLine triggerLine;
    public Animator characterAnimator;
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
    public Sequence turnSequence;
    public void TurnLeft()
    {
        if (turnSequence != null) turnSequence.Kill();

        turnSequence = DOTween.Sequence();
        turnSequence.Append(playerCharacter.GetChild(0).DOLocalRotate(new Vector3(0, 20, 0), 0.35f));
        turnSequence.Append(playerCharacter.GetChild(0).DOLocalRotate(new Vector3(0, 0, 0), 0.35f));
    }

    public void TurnRight()
    {
        if (turnSequence != null) turnSequence.Kill();

        turnSequence = DOTween.Sequence();
        turnSequence.Append(playerCharacter.GetChild(0).DOLocalRotate(new Vector3(0, -20, 0), 0.35f));
        turnSequence.Append(playerCharacter.GetChild(0).DOLocalRotate(new Vector3(0, 0, 0), 0.35f));
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
            TurnLeft();
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
            TurnLeft();
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
            TurnRight();
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
            TurnRight();
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
            
        }

    }

    private void OnEPressed()
    {
        if (isInSpecialActionTriggerE)
        {
            isInSpecialActionTriggerE = false;
            if (triggerLine != null)
            {
                Transform playerParrent = playerCharacter.parent;
                canMove = false;
                Sequence lineSequence = DOTween.Sequence();
                lineSequence.Append(playerCharacter.transform.DOJump(triggerLine.characterPivot.position, 1, 1, 1));
                lineSequence.AppendCallback(() => playerCharacter.SetParent(triggerLine.characterPivot));
                lineSequence.Append(triggerLine.linePivot.DOLocalRotate(new Vector3(-35, 0, 0), 1));
                lineSequence.Append(playerCharacter.transform.DOJump(triggerLine.endPoint.position, 1, 1, 1));
                lineSequence.AppendCallback(() => playerCharacter.SetParent(playerParrent));
                lineSequence.Join(playerCharacter.DOLocalRotate(Vector3.zero, 0.1f));
                lineSequence.AppendCallback(() => canMove = true);
                lineSequence.AppendCallback(() => triggerLine = null);
            }
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
        characterAnimator.ResetTrigger("EndJump");
        characterAnimator.SetTrigger("Jump");
        jumpSequence = DOTween.Sequence();
        jumpSequence.AppendCallback(() => isInJumpState = true);
        jumpSequence.AppendCallback(() => Salto());
        jumpSequence.Append(playerCharacter.DOLocalMoveY(jumpHigh, jumpTime).SetEase(jumpUpCurve));
        jumpSequence.AppendCallback(() => characterAnimator.SetTrigger("EndJump"));
        jumpSequence.Append(playerCharacter.DOLocalMoveY(0.25f, jumpTime).SetEase(jumpDownCurve));
        jumpSequence.AppendCallback(() => isInJumpState = false);
    }
    public float saltoInterval = 0.20f;
    public float saltoTime;
    public AnimationCurve saltoCurve;

    public Sequence saltoSequence;
    public void Salto()
    {
        saltoSequence = DOTween.Sequence();
        saltoSequence.AppendInterval(saltoInterval);
        saltoSequence.Append( playerCharacter.GetChild(0).DOLocalRotate(new Vector3(360, 0, 0), saltoTime, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear));
    }

    public void OnTriggerHole()
    {
        hp-=10;
        gameMachineSmallScreenController.ShowMessage("", 2, true, true);
        OnObstacleEnterEvent.Invoke();
    }

    public void OnObstacleEnter()
    {
        hp--;
        gameMachineSmallScreenController.ShowMessage("", 2, true, true);
        OnObstacleEnterEvent.Invoke();
    }


 
}
