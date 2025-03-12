using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public string ID;
    public PersistentType persistentType;
    private void OnValidate()//编辑器模式下生成唯一实例ID
    {
        if(persistentType==PersistentType.ReadWrite)
        {
            if(ID==string.Empty)
            ID = Guid.NewGuid().ToString();
            
        }
        else
        {
            ID = string.Empty;
        }
    }
}
