using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "FloatEventSO", menuName = "ScriptableObjects/FloatEventSO")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void Raise(float value)
    {
        OnEventRaised?.Invoke(value);
    }
}
