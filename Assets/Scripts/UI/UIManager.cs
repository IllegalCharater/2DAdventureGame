using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("事件监听")] 
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO sceneLoadEvent;
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        healthEvent.OnEventRaised += OnPowerEvent;
        sceneLoadEvent.LoadRequestEvent += OnSceneLoadEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        healthEvent.OnEventRaised -= OnPowerEvent;
        sceneLoadEvent.LoadRequestEvent -= OnSceneLoadEvent;
    }

    private void OnSceneLoadEvent(GameSceneSO sceneToGo, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToGo.sceneType == SceneType.Menu;
        playerStateBar.gameObject.SetActive(!isMenu);
        
    }

    private void OnHealthEvent(Character character)
    {
        var percentage=character.currentHealth / character.maxHealth;
        playerStateBar.OnHealthChange(percentage);
        
    }

    private void OnPowerEvent(Character character)
    {
        playerStateBar.OnPowerDisplay(character);
    }
}
