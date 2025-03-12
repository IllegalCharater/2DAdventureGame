using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    
    private IInteractable targetItem;
    
    public Animator animator;
    public Transform player;
    public GameObject signSprite;
    
    
    public bool canPress = false;

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        animator = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInputControl();
        
    }

    private void OnEnable()
    {
        playerInput.Enable();
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
        InputSystem.onActionChange -= OnActionChange;
        playerInput.Gameplay.Confirm.started -= OnConfirm;
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale=player.localScale;
    }
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponentInChildren<AudioDefination>()?.PlayAudioClip();
        }
    }
/// <summary>
/// 切换设备的同时切换交互动画
/// </summary>
/// <param name="obj"></param>
/// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    animator.Play("keyBoard");
                    break;
                case Gamepad :
                    animator.Play("ps");
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
