using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class Data
{
    public List<ObjectData> objectData;
}

[System.Serializable]
public class ObjectData
{
    public string name;
    public int x;
    public int y;

    public ObjectData(string name, int x, int y)
    {
        this.name = name;
        this.x = x;
        this.y = y;
    }
}

public class ObjectController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void setData(string str);

    public static ObjectController instance;
    public GameObject obj;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {
        Instantiate(obj, new Vector3(7,5,0), Quaternion.identity);//オブジェクト生成テスト
        CallSetData();
    }

    void Update()
    {
        Debug.Log("更新");
        CallSetData();
    }

    public void CallSetData()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        Data datas = new Data();
        datas.objectData = new List<ObjectData>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameObject");
        foreach (GameObject obj in gameObjects)
        {
            // シーン上に存在するオブジェクトならば処理.
            if (obj.activeInHierarchy)
            {
                ObjectData tmp = new ObjectData(obj.name, (int)obj.transform.position.x, (int)obj.transform.position.y);
                datas.objectData.Add(tmp);
            }
        }
        setData(JsonUtility.ToJson(datas));
#endif
    }
}
