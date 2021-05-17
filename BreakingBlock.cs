using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion1")|| other.CompareTag("Explosion2")|| other.CompareTag("Explosion3")|| other.CompareTag("Explosion4"))
        {
            Destroy(gameObject);
        }
    }
}
