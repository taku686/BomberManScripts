using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] PlayerBase player;
    [SerializeField] PlayerBase_OffLine playerBase_OffLine;



    void OnTriggerEnter(Collider other)
    {
        if (!GManager.Instance.isOffLine)
        {
            if (other.CompareTag("Explosion1") && !player.dead  && GManager.Instance.playerNum != 1)
            {
                //        Debug.Log("爆風検知");
                player.dead = true;
                player.isDead = true;
                player.int_playerNum = 1;               
            }
            else if (other.CompareTag("Explosion2") && !player.dead && GManager.Instance.playerNum != 2)
            {
                //        Debug.Log("爆風検知");
                player.dead = true;
                player.isDead = true;
                player.int_playerNum = 2;                
            }
            else if (other.CompareTag("Explosion3") && !player.dead && GManager.Instance.playerNum != 3)
            {
                //        Debug.Log("爆風検知");
                player.dead = true;
                player.isDead = true;
                player.int_playerNum = 3;
            }
            else if (other.CompareTag("Explosion4") && !player.dead && GManager.Instance.playerNum != 4)
            {
                //        Debug.Log("爆風検知");
                player.dead = true;
                player.isDead = true;
                player.int_playerNum = 4;
            }
        }
        else
        {
            if (other.CompareTag("Explosion1") && !playerBase_OffLine.dead )
            {
                //        Debug.Log("爆風検知");
                playerBase_OffLine.dead = true;
                //      playerBase_OffLine.isDead = true;
                StartCoroutine(playerBase_OffLine.Die());
            }
            else if (other.CompareTag("Explosion2") && !playerBase_OffLine.dead )
            {
                //        Debug.Log("爆風検知");
                playerBase_OffLine.dead = true;
                //      playerBase_OffLine.isDead = true;
                StartCoroutine(playerBase_OffLine.Die());
            }
            else if (other.CompareTag("Explosion3") && !playerBase_OffLine.dead )
            {
                //        Debug.Log("爆風検知");
                playerBase_OffLine.dead = true;
                //      playerBase_OffLine.isDead = true;
                StartCoroutine(playerBase_OffLine.Die());
            }
            else if (other.CompareTag("Explosion4") && !playerBase_OffLine.dead )
            {
                //        Debug.Log("爆風検知");
                playerBase_OffLine.dead = true;
                //      playerBase_OffLine.isDead = true;
                StartCoroutine(playerBase_OffLine.Die());
            }
        }

    }
}
