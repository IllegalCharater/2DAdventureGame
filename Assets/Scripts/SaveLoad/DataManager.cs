using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Newtonsoft.Json;
using System.IO;


[DefaultExecutionOrder(-100)]//优先运行
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    [Header("事件监听")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO afterLoadDataEvent;
    
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    private string jsonFolder;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        saveData = new Data();
        
        jsonFolder = Application.persistentDataPath+"/SaveData/";
        
        ReadSaveData();
    }
    

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += SaveData;
        loadDataEvent.OnEventRaised += LoadDataCoroutine;
        
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= SaveData;
        loadDataEvent.OnEventRaised -= LoadDataCoroutine;
        
    }
    
    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
            //Debug.Log("register");
        }
    }

    public void UnregisterSaveData(ISaveable saveable)
    {
        if (saveableList.Contains(saveable))
        {
            saveableList.Remove(saveable);
            //Debug.Log("unregister");
        }
    }

    public void SaveData()
    {
        
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        var resultPath=jsonFolder+"SaveData.json";
        
        var jsonData =JsonConvert.SerializeObject(saveData);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        File.WriteAllText(resultPath,jsonData);
        // foreach (var item in saveData.characterPosDict)
        // {
        //     Debug.Log(item.Key+" "+item.Value);
        // }
    }

    private void LoadDataCoroutine()
    {
        StartCoroutine(LoadData());
    }
    public IEnumerator LoadData()
    {
        // 先加载场景
        foreach (var saveable in saveableList.Where(s=>s.Priority==0))
        {
            
            saveable.LoadData(saveData);
            
        }

        // 确保场景完全加载
        yield return new WaitForSeconds(1f);

        // 再加载角色
        foreach (var saveable in saveableList.Where(s=>s.Priority==1))
        {
            
            saveable.LoadData(saveData);
            
        }
        //yield return new WaitForSeconds(1f);
        
        afterLoadDataEvent.OnEventRaised?.Invoke();
    }

    // public void LoadData()
    // {
    //     foreach (var saveable in saveableList.OrderBy(s => s.Priority))
    //     {
    //         saveable.LoadData(saveData);
    //     }
    // }

    private void ReadSaveData()
    {
        var resultPath=jsonFolder+"SaveData.json";

        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);
            
            saveData = jsonData;
        }
    }

}
