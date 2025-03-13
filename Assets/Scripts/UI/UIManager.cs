using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("事件监听")] 
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO sceneUnloadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO afterLoadDataEvent;
    public VoidEventSO gameOverEvent;
    [Header("死亡组件面板")]
    public GameObject gameOverPanel;
    public GameObject restartButton;
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        healthEvent.OnEventRaised += OnPowerEvent;
        sceneUnloadEvent.LoadRequestEvent += OnSceneUnloadEvent;
        //loadDataEvent.OnEventRaised += OnAfterLoadDataEvent;
        afterLoadDataEvent.OnEventRaised += OnAfterLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        healthEvent.OnEventRaised -= OnPowerEvent;
        sceneUnloadEvent.LoadRequestEvent -= OnSceneUnloadEvent;
        //loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        afterLoadDataEvent.OnEventRaised -= OnAfterLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
    }

    private void OnAfterLoadDataEvent()
    {
        
        gameOverPanel.SetActive(false);
        
    }

    private void OnSceneUnloadEvent(GameSceneSO sceneToGo, Vector3 arg1, bool arg2)
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
