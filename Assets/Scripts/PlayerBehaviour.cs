using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public void MoveLeft()
    {
        if (Physics.OverlapSphere(transform.position - transform.right, 0).Length <= 0)
        {
            transform.position -= transform.right;
        }
    }

    public void MoveRight()
    {

        if (Physics.OverlapSphere(transform.position + transform.right, 0).Length <= 0)
        {
            transform.position += transform.right;
        }
    }

    public void MoveForward()
    {
        if (Physics.OverlapSphere(transform.position + transform.forward, 0).Length <= 0)
        {
            transform.position += transform.forward;
        }
    }

    public void MoveBack()
    {
        if (Physics.OverlapSphere(transform.position  -  transform.forward, 0).Length <= 0)
        {
            transform.position -= transform.forward;
        }
    }
}
