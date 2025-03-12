using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SavePoint : MonoBehaviour,IInteractable
{
    
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;
    
    public Sprite spritedDark;
    public Sprite spriteLight;
    public bool isDone;
    [Header("广播")]
    public VoidEventSO saveGameEvent;
    private void OnEnable()
    {
        spriteRenderer.sprite=isDone?spriteLight:spritedDark;
        lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = spriteLight;
            gameObject.tag = "Untagged";
            lightObj.SetActive(true);
            //todo:保存数据
            saveGameEvent.RaiseEvent();
        }
    }
}
