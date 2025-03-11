using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteractable
{
    SpriteRenderer spriteRenderer;
    
    public Sprite openChest;
    public Sprite closeChest;
    public bool isOpen;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isOpen ? openChest : closeChest;
    }

    public void TriggerAction()
    {
        Debug.Log("Open Chest");
        if (!isOpen)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        spriteRenderer.sprite=openChest;
        isOpen = true;
        this.gameObject.tag = "Untagged";
    }
    
}
