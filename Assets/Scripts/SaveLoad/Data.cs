using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Data
{
    public string sceneToSave;
    //角色坐标
    public Dictionary<string,SerializableVector3> characterPosDict=new Dictionary<string,SerializableVector3>();
    //血量，耐力等
    public Dictionary<string ,float> floatSaveDate=new Dictionary<string, float>();

    public void SaveGameScene(GameSceneSO saveScene)
    {
        sceneToSave=JsonUtility.ToJson(saveScene);
        //Debug.Log(sceneToSave);
    }

    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}

public class SerializableVector3
{
    public float x,y,z;

    public SerializableVector3(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x,y,z);
    }
}