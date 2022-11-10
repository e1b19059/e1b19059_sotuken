using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private int speed = 3;
    private Vector3 targetPosition;
    private bool moving;

    Animator animator;
    bool running; // �t�B�[���h
    bool Running
    { // �v���p�e�B
        get { return running; }
        set
        { // �l���قȂ�Z�b�g���̂�animator.SetBool���ĂԂ悤�ɂ���
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
                Running = true; // �v���p�e�B�ɂ��Z�b�g
            }
            else
            {
                Running = false; // �v���p�e�B�ɂ��Z�b�g
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
