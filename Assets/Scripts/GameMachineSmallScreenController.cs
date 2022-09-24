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

    private void Start()
    {
        BlinkEffect();
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
}
