using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Photon.Pun;

public class PlayerBehaviour : MonoBehaviour
{
    int speed = 3;
    int bombCnt;
    bool moving;
    string team;
    Vector3 targetPosition;
    JSONCreator jsonCreator;
    CreateField createField;
    ObjectContainer container;

    [DllImport("__Internal")]
    private static extern void setPlayerPos(int x, int z);

    [DllImport("__Internal")]
    private static extern void setPlayerDir(int x, int z);

    Animator animator;
    bool running; // フィールド
    bool Running
    { // プロパティ
        get { return running; }
        set
        { // 値が異なるセット時のみanimator.SetBoolを呼ぶようにする
            if (value != running)
            {
                running = value;
                animator.SetBool("Running", running);
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        jsonCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<JSONCreator>();
        createField = GameObject.FindGameObjectWithTag("FloorContainer").GetComponent<CreateField>();
        container = GameObject.FindGameObjectWithTag("ObjectContainer").GetComponent<ObjectContainer>();
        bombCnt = 3;
        team = gameObject.name.Substring(0, 1);
    }

    void Update()
    {
        if (moving)
        {
            if (targetPosition != transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                Running = true; // プロパティによるセット
            }
            else
            {
                Running = false; // プロパティによるセット
            }
            setPlayerPos((int)Math.Round(transform.position.x), (int)Math.Round(transform.position.z));
            setPlayerDir((int)transform.forward.x, (int)transform.forward.z);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            Debug.Log("爆風に当たってしまった!");
            PlayerPrefs.SetInt($"Damage{team}", PlayerPrefs.GetInt($"Damage{team}") + 1);
            SetScore(-50);
        }
    }

    public void MoveLeft()
    {
        var targetPos = transform.position - transform.right;
        targetPos.y = 0;
        if (Physics.OverlapSphere(targetPos, 0).Length <= 0)
        {
            targetPosition -= transform.right;
        }
        else
        {
            SetScore(-10);
            AddMissCount();
        }
    }

    public void MoveRight()
    {
        var targetPos = transform.position + transform.right;
        targetPos.y = 0;
        if (Physics.OverlapSphere(targetPos, 0).Length <= 0)
        {
            targetPosition += transform.right;
        }
        else
        {
            SetScore(-10);
            AddMissCount();
        }
    }

    public void MoveForward()
    {
        var targetPos = transform.position + transform.forward;
        targetPos.y = 0;
        if (Physics.OverlapSphere(targetPos, 0).Length <= 0)
        {
            targetPosition += transform.forward;
        }
        else
        {
            SetScore(-10);
            AddMissCount();
        }
    }

    public void MoveBack()
    {
        var targetPos = transform.position - transform.forward;
        targetPos.y = 0;
        if (Physics.OverlapSphere(targetPos, 0).Length <= 0)
        {
            targetPosition -= transform.forward;
        }
        else
        {
            SetScore(-10);
            AddMissCount();
        }
    }

    public void TurnLeft()
    {
        transform.Rotate(0, -90f, 0);
    }

    public void TurnRight()
    {
        transform.Rotate(0, 90f, 0);
    }

    public void PutObstacle(string direction)
    {
        Vector3 targetPos = transform.position;
        switch (direction)
        {
            case "left":
                targetPos -= transform.right;
                break;
            case "right":
                targetPos += transform.right;
                break;
            case "forward":
                targetPos += transform.forward;
                break;
            case "back":
                targetPos -= transform.forward;
                break;
        }
        targetPos.y = 0;// プレイヤーキャラクターのy座標は足元にあるため他のオブジェクトに合わせる

        if (Physics.OverlapSphere(targetPos, 0.3f).Length <= 0)
        {
            createField.RPCCreateObstacle(targetPos);
        }
        else
        {
            SetScore(-10);
            AddMissCount();
        }
    }

    public void PutBomb(string direction)
    {
        if(bombCnt <= 0)
        {
            SetScore(-10);
            AddMissCount();
            return;
        }
        bombCnt--;
        Vector3 targetPos = transform.position;
        switch (direction)
        {
            case "left":
                targetPos -= transform.right;
                break;
            case "right":
                targetPos += transform.right;
                break;
            case "forward":
                targetPos += transform.forward;
                break;
            case "back":
                targetPos -= transform.forward;
                break;
        }
        targetPos.y = 0;// プレイヤーキャラクターのy座標は足元にあるため他のオブジェクトに合わせる
        if (Physics.OverlapSphere(targetPos, 0.3f).Length <= 0)
        {
            createField.RPCCreateBomb(targetPos);
        }
        else
        {
            SetScore(-10);
            AddMissCount();
        }
    }

    public void DestroyObstacle(string direction)
    {
        var enumerator = container.GetEnumerator();
        Vector3 targetPos = transform.position;
        switch (direction)
        {
            case "left":
                targetPos -= transform.right;
                break;
            case "right":
                targetPos += transform.right;
                break;
            case "forward":
                targetPos += transform.forward;
                break;
            case "back":
                targetPos -= transform.forward;
                break;
        }
        targetPos.y = 0;// プレイヤーキャラクターのy座標は足元にあるため他のオブジェクトに合わせる
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.transform.position == targetPos && enumerator.Current.gameObject.CompareTag("Destroyable"))
            {
                Destroy(enumerator.Current.gameObject);
                return;
            }
        }
        SetScore(-10);
        AddMissCount();
    }

    public void PickBomb(string direction)
    {
        var enumerator = container.GetEnumerator();
        Vector3 targetPos = transform.position;
        switch (direction)
        {
            case "left":
                targetPos -= transform.right;
                break;
            case "right":
                targetPos += transform.right;
                break;
            case "forward":
                targetPos += transform.forward;
                break;
            case "back":
                targetPos -= transform.forward;
                break;
        }
        targetPos.y = -0.1f;// 爆弾オブジェクトの高さに合わせる
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.transform.position == targetPos && enumerator.Current.gameObject.CompareTag("Bomb"))
            {
                Destroy(enumerator.Current.gameObject);
                bombCnt++;
                return;
            }
        }
        SetScore(-10);
        AddMissCount();
    }

    public void Init()
    {
        targetPosition = transform.position;
        moving = true;
        jsonCreator.StartUpdate();
    }

    public void Termi()
    {
        moving = false;
        Running = false;
        transform.position = targetPosition;
        jsonCreator.StopUpdate();
        if(bombCnt <= 3) bombCnt = 3;
    }

    public int GetBombCnt()
    {
        return bombCnt;
    }

    public void SetScore(int _score)
    {
        int result = PlayerPrefs.GetInt($"Score{team}") + _score;
        if(result < 0)
        {
            PlayerPrefs.SetInt($"Score{team}", 0);
        }
        else
        {
            PlayerPrefs.SetInt($"Score{team}", result);
        }
    }

    public void AddMissCount()
    {
        PlayerPrefs.SetInt($"Miss{team}", PlayerPrefs.GetInt($"Miss{team}") + 1);
    }

}
