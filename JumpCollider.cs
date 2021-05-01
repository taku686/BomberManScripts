using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollider : MonoBehaviour
{
    public bool isEnabledJump=true;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("BreakingWall") || other.CompareTag("OutWall"))
        {
            isEnabledJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("BreakingWall") || other.CompareTag("OutWall"))
        {
            isEnabledJump = true;
        }
    }
}
