using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "GameSceneSO", menuName = "ScriptableObjects/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public AssetReference sceneReference;//场景引用
    public SceneType sceneType;
}
