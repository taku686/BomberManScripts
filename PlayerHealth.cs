using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] PlayerBase player;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion") && !player.dead && !player.itemManager.isBarrier)
        {
            //        Debug.Log("爆風検知");
            player.dead = true;
            player.isDead = true;
        }
    }
}
