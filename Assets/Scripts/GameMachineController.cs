using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
public class GameMachineController : MonoBehaviour
{
    [SerializeField] private Transform joyStick;
    [SerializeField] private Transform aButton;
    [SerializeField] private Transform bButton;
    [SerializeField] private Transform jumpButton;

    [SerializeField] private UnityEvent APressed;
    [SerializeField] private UnityEvent DPressed;
    [SerializeField] private UnityEvent QPressed;
    [SerializeField] private UnityEvent EPressed;
    [SerializeField] private UnityEvent SpacePressed;


    [SerializeField] private float joyStickRotationTime;
    [SerializeField] private AnimationCurve joyStickRotationCurve;

    [SerializeField] private float buttonPressTime;
    [SerializeField] private AnimationCurve buttonPressCurve;


    [SerializeField] private Vector3 joyStickLeftRotation;
    [SerializeField] private Vector3 joyStickUpRotation;
    [SerializeField] private Vector3 joyStickRightRotation;
    [SerializeField] private Vector3 joyStickDownRotation;

    [SerializeField] private Vector3 aButtonPressedPosition;
    [SerializeField] private Vector3 bButtonPressedPosition;
    [SerializeField] private Vector3 jumpButtonPressedPosition;

    private Sequence joyStickSequence;

    [SerializeField] private Light screenLight;
    [SerializeField] private Light aLight;
    [SerializeField] private Light bLight;
    [SerializeField] private Light spaceLight;
    [SerializeField] private float LightOnIntensity = 0.01f;


    private Sequence aButtonSequence;
    private Sequence bButtonSequence;
    private Sequence jumpButtonSequence;

    private Vector3 aButtonDefaultPosition;
    private Vector3 bButtonDefaultPosition;
    private Vector3 jumpButtonDefaultPosition;
    private Vector3 joyStickDefaultRotation;
    public Material buttonsMaterial;
    [SerializeField] private Texture2D texture;
    [SerializeField] private Image splashScreen;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameMachineSmallScreenController machineSmallScreenController;
    private Sequence blinkSequence;
    [SerializeField] private float lightHighColor;
    [SerializeField] private float lightLowColor;

    [SerializeField] private float blinkTime;
    [SerializeField] private AnimationCurve blinkCurve;
    [SerializeField] private Color splashScreenTargetColor;
    [SerializeField] private float splashScreenTargetColorTime = 1;
    [SerializeField] private AnimationCurve splashScreenTargetColorCurve;
    [SerializeField] private Transform gameMachineCamera;
    [SerializeField] public CanvasGroup ranking;

    private void Start()
    {
        StartTimeline();


        StartCoroutine(SplashScreen());
        buttonsMaterial.EnableKeyword("_EMISSION");
        joyStickDefaultRotation = joyStick.localEulerAngles;
        aButtonDefaultPosition = aButton.localPosition;
        bButtonDefaultPosition = bButton.localPosition;
        jumpButtonDefaultPosition = jumpButton.localPosition;
        BlinkEffect();
    }
    private Sequence timelineSequence;
    public Vector3 targetCameraPosition;
    public float targetCameraPositionTime;
    public AnimationCurve targetCameraPositionCurve;
    public void StartTimeline()
    {
        gameMachineCamera.transform.localPosition = new Vector3(0, 0, 3);
        splashScreen.gameObject.SetActive(true);
        splashScreen.color = Color.black;
        timelineSequence = DOTween.Sequence();
        timelineSequence.AppendInterval(2);
        timelineSequence.Append(gameMachineCamera.DOLocalMove(targetCameraPosition, targetCameraPositionTime).SetEase(targetCameraPositionCurve));
        timelineSequence.AppendInterval(1);
        timelineSequence.AppendCallback(() => machineSmallScreenController.ShowMessage("Welcome!", 2, false));
        timelineSequence.AppendInterval(2.5f);
        timelineSequence.Append(splashScreen.DOColor(splashScreenTargetColor, splashScreenTargetColorTime).SetEase(splashScreenTargetColorCurve));
        timelineSequence.AppendCallback(() => machineSmallScreenController.ShowMessage("Press Jump", 2, false));
        timelineSequence.AppendCallback(() => SplashScreen());
    }



    public void BlinkEffect()
    {
        screenLight.intensity = lightLowColor;
        blinkSequence = DOTween.Sequence();
        blinkSequence.Append(screenLight.DOIntensity(lightHighColor, blinkTime).SetEase(blinkCurve));
        blinkSequence.Append(screenLight.DOIntensity(lightLowColor, blinkTime).SetEase(blinkCurve));
        blinkSequence.SetLoops(-1, LoopType.Restart);
    }
    IEnumerator SplashScreen()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        machineSmallScreenController.ShowMessage("Start !", 1, true);
        if (timelineSequence != null)
        {
            timelineSequence.Kill();
        }
        splashScreen.DOFade(0, 1);
        characterController.canMove = true;
    }


    private void Update()
    {


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
    [ColorUsage(true, true)]
    public Color EmissionHigh;
    [ColorUsage(true, true)]
    public Color EmissionLow;
    private void OnMoveLeft()
    {
        if (joyStickSequence != null)
        {
            joyStickSequence.Kill();
        }
        joyStickSequence = DOTween.Sequence();

        joyStickSequence.Append(joyStick.DOLocalRotate(joyStickLeftRotation, joyStickRotationTime).SetEase(joyStickRotationCurve));

        joyStickSequence.Append(joyStick.DOLocalRotate(joyStickDefaultRotation, joyStickRotationTime).SetEase(joyStickRotationCurve));

    }

    private void OnMoveRight()
    {
        if (joyStickSequence != null)
        {
            joyStickSequence.Kill();
        }
        joyStickSequence = DOTween.Sequence();
        joyStickSequence.Append(joyStick.DOLocalRotate(joyStickRightRotation, joyStickRotationTime).SetEase(joyStickRotationCurve));
        joyStickSequence.Append(joyStick.DOLocalRotate(joyStickDefaultRotation, joyStickRotationTime).SetEase(joyStickRotationCurve)).OnComplete(() => joyStickSequence.Kill());

    }

    private void OnQPressed()
    {
        if (aButtonSequence != null)
        {
            aButtonSequence.Kill();
        }
        aButtonSequence = DOTween.Sequence();
        aButtonSequence.Append(aButton.DOLocalMove(aButtonPressedPosition, buttonPressTime).SetEase(buttonPressCurve));
        aButtonSequence.Join(aButton.GetComponent<MeshRenderer>().materials[0].DOColor(EmissionHigh, "_EmissionColor", buttonPressTime * 2).SetEase(buttonPressCurve));
        aButtonSequence.Join(aLight.DOIntensity(LightOnIntensity, buttonPressTime * 2).SetEase(buttonPressCurve));
        aButtonSequence.Append(aButton.DOLocalMove(aButtonDefaultPosition, buttonPressTime).SetEase(buttonPressCurve));
        aButtonSequence.Join(aLight.DOIntensity(0.00f, buttonPressTime * 2).SetEase(buttonPressCurve));
        aButtonSequence.Join(aButton.GetComponent<MeshRenderer>().materials[0].DOColor(EmissionLow, "_EmissionColor", buttonPressTime * 2).SetEase(buttonPressCurve)).OnComplete(() => aButtonSequence.Kill());
    }

    private void OnEPressed()
    {
        if (bButtonSequence != null)
        {
            bButtonSequence.Kill();
        }
        bButtonSequence = DOTween.Sequence();
        bButtonSequence.Append(bButton.DOLocalMove(bButtonPressedPosition, buttonPressTime).SetEase(buttonPressCurve));
        bButtonSequence.Join(bLight.DOIntensity(LightOnIntensity, buttonPressTime * 2).SetEase(buttonPressCurve));
        bButtonSequence.Join(bButton.GetComponent<MeshRenderer>().materials[0].DOColor(EmissionHigh, "_EmissionColor", buttonPressTime * 2).SetEase(buttonPressCurve));
        bButtonSequence.Append(bButton.DOLocalMove(bButtonDefaultPosition, buttonPressTime).SetEase(buttonPressCurve));
        bButtonSequence.Join(bLight.DOIntensity(0.00f, buttonPressTime * 2).SetEase(buttonPressCurve));
        bButtonSequence.Join(bButton.GetComponent<MeshRenderer>().materials[0].DOColor(EmissionLow, "_EmissionColor", buttonPressTime * 2).SetEase(buttonPressCurve)).OnComplete(() => bButtonSequence.Kill());
    }

    private void OnSpacePressed()
    {
        if (jumpButtonSequence != null)
        {
            jumpButtonSequence.Kill();
        }
        jumpButtonSequence = DOTween.Sequence();
        jumpButtonSequence.Append(jumpButton.DOLocalMove(jumpButtonPressedPosition, buttonPressTime).SetEase(buttonPressCurve));
        jumpButtonSequence.Join(spaceLight.DOIntensity(LightOnIntensity, buttonPressTime * 2).SetEase(buttonPressCurve));
        jumpButtonSequence.Join(jumpButton.GetComponent<MeshRenderer>().materials[0].DOColor(EmissionHigh, "_EmissionColor", buttonPressTime * 2).SetEase(buttonPressCurve));
        jumpButtonSequence.Append(jumpButton.DOLocalMove(jumpButtonDefaultPosition, buttonPressTime).SetEase(buttonPressCurve)).OnComplete(() => jumpButtonSequence.Kill());
        jumpButtonSequence.Join(spaceLight.DOIntensity(0.00f, buttonPressTime * 2).SetEase(buttonPressCurve));
        jumpButtonSequence.Join(jumpButton.GetComponent<MeshRenderer>().materials[0].DOColor(EmissionLow, "_EmissionColor", buttonPressTime * 2).SetEase(buttonPressCurve)).OnComplete(() =>
        {
            bButtonSequence.Kill();
        });
    }
}
