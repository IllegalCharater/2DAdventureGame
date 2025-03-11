using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "FadeEventSO", menuName = "ScriptableObjects/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color,float,bool> OnEventRaised;
    /// <summary>
    /// 场景变黑
    /// </summary>
    /// <param name="duration"></param>
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }
    /// <summary>
    /// 场景变透明
    /// </summary>
    /// <param name="duration"></param>
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }

    public void RaiseEvent(Color color,float duration,bool fadeIn)
    {
        OnEventRaised?.Invoke(color,duration,fadeIn);
    }
}
