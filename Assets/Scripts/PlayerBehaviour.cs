using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Photon.Pun;

public class PlayerBehaviour : MonoBehaviour
{
    int speed = 3;
    bool moving;
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
        createField = GameObject.Find("FloorContainer").GetComponent<CreateField>();
        container = GameObject.Find("ObjectContainer").GetComponent<ObjectContainer>();
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
            var team = gameObject.name.Substring(0, 1);
            PlayerPrefs.SetInt($"Score{team}", PlayerPrefs.GetInt($"Score{team}") - 1);
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
    }

    public void MoveRight()
    {
        var targetPos = transform.position + transform.right;
        targetPos.y = 0;
        if (Physics.OverlapSphere(targetPos, 0).Length <= 0)
        {
            targetPosition += transform.right;
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
    }

    public void MoveBack()
    {
        var targetPos = transform.position - transform.forward;
        targetPos.y = 0;
        if (Physics.OverlapSphere(targetPos, 0).Length <= 0)
        {
            targetPosition -= transform.forward;
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
    }

    public void PutBomb(string direction)
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
            createField.RPCCreateBomb(targetPos);
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
                break;
            }
        }
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
    }

}
