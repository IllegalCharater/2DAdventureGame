using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterEventSO", menuName = "ScriptableObjects/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;
    
    public void RaiseEvent(Character character)
    {
        OnEventRaised?.Invoke(character);
    }
}
