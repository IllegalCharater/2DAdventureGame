using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class InitalLoad : MonoBehaviour
{
    public AssetReference persistentAsset;

    private void Awake()
    {
        Addressables.LoadSceneAsync(persistentAsset);
    }
}
