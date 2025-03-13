using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    int Priority { get; }  // 让存档对象有优先级
    DataDefination GetDataID();
    void RegisterSaveData() =>DataManager.instance.RegisterSaveData(this);
    
    void UnregisterSaveData() => DataManager.instance.UnregisterSaveData(this);
    
    void GetSaveData(Data data);
    void LoadData(Data data);
}
