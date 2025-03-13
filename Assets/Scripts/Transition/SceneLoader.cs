using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    [Header("事件监听")] 
    public SceneLoadEventSO LoadEventSO;
    public VoidEventSO newGameEvent;
    
    private GameSceneSO currentScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool isLoading;
    private bool fadeScene;
    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public float fadeTime;
    public SceneLoadEventSO sceneUnloadEvent;
    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private void Awake()
    {
        // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);//静态类中的加载方法与下面的方法不同
        // currentScene = firstLoadScene;
        // currentScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        
    }

    private void Start()
    {
        // NewGameStart();
        LoadEventSO.RaiseLoadRequestEvent(menuScene,menuPosition,true);
    }

    private void OnEnable()
    {
        LoadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGameStart;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        LoadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGameStart;
        ISaveable saveable = this;
        saveable.UnregisterSaveData();
    }

    //开始新游戏
    private void NewGameStart()
    {
        sceneToLoad = firstLoadScene;
        // OnLoadRequestEvent(sceneToLoad,firstPosition,true);
        LoadEventSO.RaiseLoadRequestEvent(sceneToLoad,firstPosition,true);
    }
    /// <summary>
    /// 加载场景请求
    /// </summary>
    /// <param name="scene">加载的新场景</param>
    /// <param name="position">加载后的位置</param>
    /// <param name="fadeScene">是否淡出</param>
    private void OnLoadRequestEvent(GameSceneSO scene, Vector3 position , bool fadeScene)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = scene;
        positionToGo = position;
        this.fadeScene = fadeScene;
        
        if(currentScene != null)
            StartCoroutine(UnloadScene());
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnloadScene()
    {
        if (fadeScene)
        {
            //变黑
            fadeEvent.FadeIn(fadeTime);
        }
        yield return new WaitForSeconds(fadeTime);
        
        //用广播事件调整血条
        sceneUnloadEvent.LoadRequestEvent(sceneToLoad,positionToGo,true);
        
        yield return currentScene.sceneReference.UnLoadScene();
        
        //关闭人物
        playerTrans.gameObject.SetActive(false);
        //加载新场景
        LoadNewScene();
    }

    void LoadNewScene()
    {
        var loadingOption
            =sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }
/// <summary>
/// 场景加载结束后执行
/// </summary>
/// <param name="obj"></param>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentScene = sceneToLoad;
        
        playerTrans.position=positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScene)
        {
            //变亮
            fadeEvent.FadeOut(fadeTime);
        }
        isLoading = false;
        
        if(currentScene.sceneType==SceneType.Location)
        {
            //场景加载完成后事件
            afterSceneLoadedEvent.RaiseEvent();
        }
    }

    public int Priority => 0;

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentScene);
    }

    public void LoadData(Data data) 
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID];
            sceneToLoad = data.GetSavedScene();
            OnLoadRequestEvent(sceneToLoad,positionToGo,true);
        }
    }
}
