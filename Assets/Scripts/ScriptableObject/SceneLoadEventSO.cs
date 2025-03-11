using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SceneLoadEventSO", menuName = "ScriptableObjects/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO,Vector3,bool> LoadRequestEvent;

    /// <summary>
    /// 加载场景需求
    /// </summary>
    /// <param name="scene">要加载的场景</param>
    /// <param name="position">目的坐标</param>
    /// <param name="fadeScreen">是否渐入渐出</param>
    public void RaiseLoadRequestEvent(GameSceneSO scene, Vector3 position, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(scene, position, fadeScreen);
    }
}
