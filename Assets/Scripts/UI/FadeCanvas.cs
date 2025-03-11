using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;
    [Header("事件监听")]
    public FadeEventSO fadeEvent;

    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }
    

    private void OnFadeEvent(Color color,float duration,bool fadeIn)
    {
        fadeImage.DOBlendableColor(color, duration);
    }
}
