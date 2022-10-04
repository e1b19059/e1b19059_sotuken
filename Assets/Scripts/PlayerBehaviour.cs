using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private int speed = 5;
    private float distance;
    private Vector3 move;
    private Vector3 targetPos;

    void Start()
    {
        distance = 1f;
        targetPos = transform.position;
    }

    void FixedUpdate()
    {
        if (move != Vector3.zero && transform.position == targetPos)
        {
            targetPos += new Vector3(move.x * distance, move.y * distance, 0);
        }
        Move(targetPos);
        move.x = 0;
        move.y = 0;
    }

    public void Move(Vector3 targetPosition)
    {
        if (Physics.OverlapSphere(targetPosition, 0).Length > 0)
        {
            // targetPos‚ªCollider‚ÉG‚ê‚Ä‚¢‚éê‡
            targetPos = transform.position;
        }
        else
        {
            // targetPos‚ªCollider‚ÉG‚ê‚Ä‚¢‚È‚¢ê‡
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    public void go_left()
    {
        move.x = -1;
    }

    public void go_right()
    {
        move.x = 1;
    }

    public void go_up()
    {
        move.y = 1;
    }

    public void go_down()
    {
        move.y = -1;
    }
}
