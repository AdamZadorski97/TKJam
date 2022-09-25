using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameMachineSmallScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text screenText;
    [SerializeField] private Color textHighColor;
    [SerializeField] private Color textLowColor;
    [SerializeField] private float blinkTime;
    [SerializeField] private AnimationCurve blinkCurve;
    private Sequence blinkSequence;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private CharacterController characterController;
    public bool canUpdateScore = false;
    private Sequence messageSequence;
    public GameObject HpPanel;
    public List<GameObject> hpIcons;
    public float points;
    public GameMachineController gameMachineController;
    private void Start()
    {
        BlinkEffect();
    }

    public void Update()
    {
        if(characterController.canMove)
        points += Time.deltaTime * 3;

        if (canUpdateScore)
        { 
            WriteText(points.ToString("0")); 
        }
           
       
    }





    public void BlinkEffect()
    {
        screenText.color = textLowColor;
        blinkSequence = DOTween.Sequence();
        blinkSequence.Append(screenText.DOColor(textHighColor, blinkTime).SetEase(blinkCurve));
        blinkSequence.Append(screenText.DOColor(textLowColor, blinkTime).SetEase(blinkCurve));
        blinkSequence.SetLoops(-1, LoopType.Restart);
    }


    public void WriteText(string message)
    {
        screenText.text = message;
    }

    IEnumerator  SetReset()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        Application.LoadLevel(0);
    }

    public void ShowMessage(string message, float time, bool canUpdateTimeAfter, bool showHP= false)
    {
        if(messageSequence!=null)
        {
            messageSequence.Kill();
        }
        messageSequence = DOTween.Sequence();
        messageSequence.AppendCallback(() => canUpdateScore = false);
        if (showHP)
        {
            if(characterController.hp>0)
            {
                HpPanel.SetActive(true);
                hpImages();
            }
            else
            {
                characterController.characterAnimator.SetTrigger("Jump");
                ShowMessage("Game Over", 2, false);
                gameMachineController.ranking.DOFade(1, 1);
                characterController.canMove = false;
                StartCoroutine(SetReset());
            }
        }
       
        else
            HpPanel.SetActive(false);
        messageSequence.AppendCallback(() => WriteText(message));
        messageSequence.AppendInterval(time);
        messageSequence.AppendCallback(() => 
        { 
            canUpdateScore = canUpdateTimeAfter;
            HpPanel.SetActive(false);
        });
    }

    private void hpImages()
    {
        if(characterController.hp == 3)
        {
            hpIcons[0].SetActive(true);
            hpIcons[1].SetActive(true);
            hpIcons[2].SetActive(true);
        }
        else if (characterController.hp == 2)
        {
            hpIcons[0].SetActive(true);
            hpIcons[1].SetActive(false);
            hpIcons[2].SetActive(true);
        }
        else if (characterController.hp == 1)
        {
            hpIcons[0].SetActive(false);
            hpIcons[1].SetActive(true);
            hpIcons[2].SetActive(false);
        }
    }
}
