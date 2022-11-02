using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Photon.Pun;

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

public class JSONCreator : MonoBehaviourPunCallbacks
{
    [DllImport("__Internal")]
    private static extern void setData(string str);

    [SerializeField] private GameManager gameManager;
    [SerializeField] private ObjectContainer container;

    private bool updateFlag = false;

    void Update()
    {
        if (updateFlag)
        {
            CallSetData();
        }
    }

    private void CallSetData()
    {
        var enumerator = container.GetEnumerator();
        Data datas = new Data();
        datas.objectData = new List<ObjectData>();
        while (enumerator.MoveNext())
        {
            Position pos = new Position((int)enumerator.Current.transform.position.x, (int)enumerator.Current.transform.position.y, (int)enumerator.Current.transform.position.z);
            Direction dir = new Direction(enumerator.Current.transform.forward);
            ObjectData tmp = new ObjectData(enumerator.Current.name, pos, dir);
            datas.objectData.Add(tmp);
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        setData(JsonUtility.ToJson(datas));
#endif
    }

    public void StartUpdate()
    {
        Debug.Log("start!");
        updateFlag = true;
    }

    public void StopUpdate()
    {
        Debug.Log("stop!");
        updateFlag = false;
        photonView.RPC(nameof(gameManager.CreateCoin), RpcTarget.MasterClient);
        photonView.RPC(nameof(gameManager.StartTurn), RpcTarget.AllViaServer);
    }

}
