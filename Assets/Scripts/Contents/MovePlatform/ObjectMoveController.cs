using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveController : MonoBehaviour
{
    [SerializeField]
    private Vector3[] movePos;
    private int moveNum = 0;
    private Vector3 moveDir;
    private bool isMoveOrder = true;
    [SerializeField]
    private List<GameObject> user;
    [SerializeField]
    private float speed;

    private void Start()
    {
        movePos[0] = transform.position;
    }

    private void FixedUpdate()
    {
        if (movePos.Length != 0)
        {
            moveDir = (movePos[moveNum] - transform.position).normalized;

            if (Vector3.Distance(transform.position, movePos[moveNum]) >= speed)
            {
                //Debug.Log($"움직임 speed / 100 : {speed / 100} \nDistance : {Vector3.Distance(transform.position, movePos[moveNum])}");
                transform.position += moveDir * speed;

            }
            else
            {
                //Debug.Log($"안 움직임 speed / 100 : {speed / 100} \nDistance : {Vector3.Distance(transform.position, movePos[moveNum])}");
                if (isMoveOrder)
                    moveNum++;
                else
                    moveNum--;
            }


            if (movePos.Length == moveNum + 1)
            {
                isMoveOrder = false;
            }
            else if (moveNum == 0)
            {
                isMoveOrder = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }

    void addCollision(Collision collision)
    {
        int dontNull = 0;

        for (int i = 0; i < user.Count; i++)
        {
            if (user[i] == null)
            {
                user[i] = collision.gameObject;
                i = user.Count;
            }
            else
                dontNull++;
        }
        if (dontNull == user.Count)
        {
            user.Add(collision.gameObject);
        }
    }
    void removeCollision(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < user.Count; i++)
            {
                if (user[i].name == collision.gameObject.name)
                {
                    user[i] = null;
                }
            }
        }
    }
    void moveCollision(Collision collision)
    { 
                for (int i = 0; i < user.Count; i++)
                {
                    if(user[i] != null)
                    user[i].gameObject.transform.position += moveDir * speed;
                }
                
    }
}
