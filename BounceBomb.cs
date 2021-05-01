using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BounceBomb : Bomb
{
    private bool isCollision;
    private bool isMove;

    private Vector3 moveDir;
   [SerializeField] private float speed;

    private void Update()
    {
        transform.position += moveDir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isMove&&m_isKick)
        {
            isMove = true;
            float angle = (collision.transform.rotation.eulerAngles.y + 1080) % 360;
            if (angle == 0)
            {
                moveDir = new Vector3(0, 0, transform.forward.z * speed);
            }
            else if (angle == 90)
            {
                moveDir = new Vector3(transform.right.x * speed, 0, 0);
            }
            else if (angle == 180)
            {
                moveDir = new Vector3(0, 0, transform.forward.z * -speed);
            }
            else if (angle == 270)
            {
                moveDir = new Vector3(transform.right.x * -speed, 0, 0);
            }
        }
        else if (collision.gameObject.CompareTag("Player") && isMove)
        {
            isMove = false;
            moveDir = Vector3.zero;
        }
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("BreakingWall")) && !isCollision&&isMove)
        {
            isCollision = true;
            StartCoroutine(Collision());
            Debug.Log(-moveDir);
            moveDir = -moveDir;
        }

    }

    IEnumerator Collision()
    {
        yield return new WaitForSeconds(0.5f);
        isCollision = false;
    }
}
