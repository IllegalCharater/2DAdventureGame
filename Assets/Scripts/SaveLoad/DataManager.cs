using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]//优先运行
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    [Header("事件监听")]
    public VoidEventSO saveDataEvent;
    
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
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
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadData();
        }
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += SaveData;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= SaveData;
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
            saveable.GetSaveDate(saveData);
        }

        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key+" "+item.Value);
        }
    }

    public void LoadData()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }
}
