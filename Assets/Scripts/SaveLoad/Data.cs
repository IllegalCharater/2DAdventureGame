using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string sceneToSave;
    //角色坐标
    public Dictionary<string,Vector3> characterPosDict=new Dictionary<string, Vector3>();
    //血量，耐力等
    public Dictionary<string ,float> floatSaveDate=new Dictionary<string, float>();

    public void SaveGameScene(GameSceneSO saveScene)
    {
        sceneToSave=JsonUtility.ToJson(saveScene);
        Debug.Log(sceneToSave);
    }

    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}
