using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefination GetDataID();
    void RegisterSaveData() =>DataManager.instance.RegisterSaveData(this);
    
    void UnregisterSaveData() => DataManager.instance.UnregisterSaveData(this);
    
    void GetSaveDate(Data data);
    void LoadData(Data data);
}
