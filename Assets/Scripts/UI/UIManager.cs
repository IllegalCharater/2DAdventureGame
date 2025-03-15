using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("事件监听")] 
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO sceneUnloadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO afterLoadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    [Header("事件广播")] 
    public VoidEventSO pauseEvent;
    [Header("死亡组件面板")]
    public GameObject gameOverPanel;
    public GameObject restartButton;
    [Header("设置面板及按钮")] 
    public Button settingButton;
    public GameObject settingPanel;
    [Header("屏幕触控")] 
    public GameObject mobileTouch;

    private void Awake()
    {
        settingButton.onClick.AddListener(ToggleSetting);
    }

    private void ToggleSetting()
    {
        if (settingPanel.activeInHierarchy)
        {
            settingPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            settingPanel.SetActive(true);
            pauseEvent.OnEventRaised();
            Time.timeScale = 0;
        }
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        healthEvent.OnEventRaised += OnPowerEvent;
        sceneUnloadEvent.LoadRequestEvent += OnSceneUnloadEvent;
        //loadDataEvent.OnEventRaised += OnAfterLoadDataEvent;
        afterLoadDataEvent.OnEventRaised += OnAfterLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        

#if UNITY_ANDROID
        sceneUnloadEvent.LoadRequestEvent += Android_SceneUnloadEvent;
        gameOverEvent.OnEventRaised += Android_GameOverEvent;
#endif
    }

    

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        healthEvent.OnEventRaised -= OnPowerEvent;
        sceneUnloadEvent.LoadRequestEvent -= OnSceneUnloadEvent;
        //loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        afterLoadDataEvent.OnEventRaised -= OnAfterLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        
        
        #if UNITY_ANDROID
        sceneUnloadEvent.LoadRequestEvent -= Android_SceneUnloadEvent;
        gameOverEvent.OnEventRaised -= Android_GameOverEvent;
        #endif
    }
    
    private void MenuClose()
    {
        gameOverPanel.SetActive(false);
    }
    private void OnBackToMenuEvent()
    {
        MenuClose();
    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
        //Debug.Log("panel invoke");
    }

    private void OnAfterLoadDataEvent()
    {
        
        MenuClose();
        
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
    
#if UNITY_ANDROID
    private void Android_GameOverEvent()
    {
        mobileTouch.SetActive(false);
    }

    private void Android_SceneUnloadEvent(GameSceneSO sceneToGo, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToGo.sceneType == SceneType.Menu;
        mobileTouch.gameObject.SetActive(!isMenu);
    }
#endif
}
