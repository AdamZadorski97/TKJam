using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class GameMachineSmallScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text screenText;
    [SerializeField] private Color textHighColor;
    [SerializeField] private Color textLowColor;
    [SerializeField] private float blinkTime;
    [SerializeField] private AnimationCurve blinkCurve;
    private Sequence blinkSequence;
    [SerializeField] private Transform playerPosition;
    public bool canUpdateScore = false;
    private Sequence messageSequence;


    private void Start()
    {
        ShowMessage("Press Jump!", 3, false);
        BlinkEffect();
    }

    public void Update()
    {
        if(canUpdateScore)
        WriteText(playerPosition.position.z.ToString("0"));
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

    public void ShowMessage(string message, float time, bool canUpdateTimeAfter)
    {
        if(messageSequence!=null)
        {
            messageSequence.Kill();
        }
        messageSequence = DOTween.Sequence();
        messageSequence.AppendCallback(() => canUpdateScore = false);
        messageSequence.AppendCallback(() => WriteText(message));
        messageSequence.AppendInterval(time);
        messageSequence.AppendCallback(() => canUpdateScore = canUpdateTimeAfter);
    }
}
