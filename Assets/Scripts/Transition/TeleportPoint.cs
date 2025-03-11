using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour,IInteractable
{
    public SceneLoadEventSO LoadEventSo;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        Debug.Log("teleport");
        
        LoadEventSo.RaiseLoadRequestEvent(sceneToGo, positionToGo,true);
    }
}
