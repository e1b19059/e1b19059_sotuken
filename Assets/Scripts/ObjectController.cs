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
    public Position position;
    public Direction direction;

    public ObjectData(string name, Position position, Direction direction)
    {
        this.name = name;
        this.position = position;
        this.direction = direction;
    }
}

[System.Serializable]
public class Position
{
    public int x;
    public int y;
    public int z;

    public Position(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[System.Serializable]
public class Direction
{
    public int x;
    public int y;
    public int z;

    public Direction(Vector3 direction)
    {
        this.x = (int)direction.x;
        this.y = (int)direction.y;
        this.z = (int)direction.z;
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
        Instantiate(obj, new Vector3(-1f,0.5f,1f), Quaternion.identity);//オブジェクト生成テスト
        Instantiate(obj, new Vector3(1f, 0.5f, 1f), Quaternion.identity);//オブジェクト生成テスト
        Instantiate(obj, new Vector3(1f, 0.5f, 2f), Quaternion.identity);//オブジェクト生成テスト
        CallSetData();
    }

    void Update()
    {
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
                Position pos = new Position((int)obj.transform.position.x, (int)obj.transform.position.y, (int)obj.transform.position.z);
                Direction dir = new Direction(obj.transform.forward);
                ObjectData tmp = new ObjectData(obj.name, pos, dir);
                datas.objectData.Add(tmp);
            }
        }
        setData(JsonUtility.ToJson(datas));
#endif
    }
}
