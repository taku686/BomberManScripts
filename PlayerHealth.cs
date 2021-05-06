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
            if (other.CompareTag("Explosion") && !player.dead && !player.itemManager.isBarrier)
            {
                //        Debug.Log("爆風検知");
                player.dead = true;
                player.isDead = true;
            }
        }
        else
        {
            if (other.CompareTag("Explosion") && !playerBase_OffLine.dead && !playerBase_OffLine.itemManager.isBarrier)
            {
                //        Debug.Log("爆風検知");
                playerBase_OffLine.dead = true;
               playerBase_OffLine.isDead = true;
            }
        }
       
    }
}
