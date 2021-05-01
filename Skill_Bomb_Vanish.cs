using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Bomb_Vanish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Bomb"))
        {
            Destroy(other.gameObject);
        }
    }
}
