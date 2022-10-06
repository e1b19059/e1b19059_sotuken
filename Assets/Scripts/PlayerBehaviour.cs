using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private int speed = 3;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
        Debug.Log(transform.forward);
    }

    void Update()
    {
        if (targetPosition != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    public void MoveLeft()
    {
        if (Physics.OverlapSphere(transform.position - transform.right, 0).Length <= 0)
        {
            targetPosition -= transform.right;
            Debug.Log(targetPosition);
        }
    }

    public void MoveRight()
    {

        if (Physics.OverlapSphere(transform.position + transform.right, 0).Length <= 0)
        {
            targetPosition += transform.right;
            Debug.Log(targetPosition);
        }
    }

    public void MoveForward()
    {
        if (Physics.OverlapSphere(transform.position + transform.forward, 0).Length <= 0)
        {
            targetPosition += transform.forward;
            Debug.Log(targetPosition);
        }
    }

    public void MoveBack()
    {
        if (Physics.OverlapSphere(transform.position  -  transform.forward, 0).Length <= 0)
        {
            targetPosition -= transform.forward;
            Debug.Log(targetPosition);
        }
    }
}
