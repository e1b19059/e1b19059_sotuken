using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Photon.Pun;

public class PlayerBehaviour : MonoBehaviour
{
    private int speed = 3;
    private Vector3 targetPosition;
    private bool moving;

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
        Debug.Log("爆発に当たった");
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.CompareTag("Explosion"))
            {
                Debug.Log("スコア減らした");
                if (gameObject.name.Substring(0, 1) == "A")// 先頭の文字を比較
                {
                    PhotonNetwork.CurrentRoom.SetScoreA(PhotonNetwork.CurrentRoom.GetScoreA() - 1);
                }
                else
                {
                    PhotonNetwork.CurrentRoom.SetScoreB(PhotonNetwork.CurrentRoom.GetScoreB() - 1);
                }
            }
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

    public void Init()
    {
        targetPosition = transform.position;
        moving = true;
    }

    public void Termi()
    {
        moving = false;
    }

}
