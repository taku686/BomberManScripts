using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class PlayerLiube : PlayerBase
{
    [SerializeField] GameObject TimeLine;
 //   [SerializeField] GameObject effect_skill_one;
    [SerializeField] GameObject effect_skill_two;
    [SerializeField] Transform effect_pos;
    [SerializeField] Transform effect_Pos2;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] JumpCollider jumpCollider;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] TrailsFX.TrailEffect trailEffect;

    protected override void Skill_One()
    {
        if (!jumpCollider.isEnabledJump)
        {
            return;
        }
        base.Skill_One();
        float angle = (transform.rotation.eulerAngles.y + 1080) % 360;
        Vector3 targetPosition = Vector3.zero;
        float jumpSpeed = 0.5f;
        photonView.RPC(nameof(Skill_One_Rpc), RpcTarget.All);
        if (angle == 0)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z + 2));            
        }
        else if (angle == 90)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x + 2), 0, Mathf.RoundToInt(transform.position.z));
        }
        else if (angle == 180)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z - 2));
        }
        else if (angle == 270)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x - 2), 0, Mathf.RoundToInt(transform.position.z));
        }
        transform.DOJump(targetPosition, 2.5f, 1, jumpSpeed);
    }

    [PunRPC]
    private void Skill_One_Rpc()
    {
        StartCoroutine(Skill_One_Corutine());
    }

    IEnumerator Skill_One_Corutine()
    {
        trailEffect.active = true;
        yield return new WaitForSeconds(.5f);
        trailEffect.active = false;
    }

    [PunRPC]
    protected override void Skill_Two()
    {
        base.Skill_Two();
        isActive_Skill_Two = true;
        StartCoroutine(Skill_Two_Activate());  
        GameObject timeline = Instantiate(TimeLine);
        Destroy(timeline, 2.5f);
    }

    IEnumerator Skill_Two_Activate()
    {
        moveSpeed = 10;
        boxCollider.isTrigger = true;
        GameObject effect = Instantiate(effect_skill_two, effect_Pos2.position, effect_skill_two.transform.rotation, effect_Pos2);
        yield return new WaitForSeconds(20);
        Destroy(effect);
        moveSpeed = 5;
        isActive_Skill_Two = false;
        boxCollider.isTrigger = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("BlockWall")&&isActive_Skill_Two)
        {
            Debug.Log("トリガーオン");
            boxCollider.isTrigger = false;
        
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("BlockWall") && isActive_Skill_Two)
        {
            Debug.Log("トリガーオフ");
            boxCollider.isTrigger = true;
        }
    }
}
