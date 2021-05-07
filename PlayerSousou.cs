using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class PlayerSousou : PlayerBase
{
    [SerializeField] GameObject effect_skill_two;
    [SerializeField] Transform effect_pos;
    private GameObject effectClone;
    private GameObject effectClone_Two;
    private float speed;
    private int firepower;
    private int bombCount;
    [SerializeField] JumpCollider jumpCollider;
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
        //     Debug.Log(angle);
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
        transform.DOJump(targetPosition, 2f, 1, jumpSpeed);
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
        StartCoroutine(Skill_Two_Activate());
        if (photonView.IsMine)
        {
            StartCoroutine(StatusUp());
        }
    }

    IEnumerator StatusUp()
    {
        yield return new WaitForSeconds(2);
        isActive_Skill_Two = true;
        speed = itemManager.speed;
        firepower = itemManager.firePower;
        bombCount = itemManager.bombCount;
        itemManager.speed = 8;
        itemManager.firePower = 8;
        itemManager.bombCount = 8;
        yield return new WaitForSeconds(20);
        itemManager.speed = speed;
        itemManager.firePower = firepower;
        itemManager.bombCount = bombCount;
        isActive_Skill_Two = false;
    }

    IEnumerator Skill_Two_Activate()
    {
        yield return new WaitForSeconds(2);      
        effectClone_Two = Instantiate(effect_skill_two, effect_pos.position, transform.rotation);
        effectClone_Two.transform.SetParent(effect_pos);
        yield return new WaitForSeconds(20);      
        Destroy(effectClone_Two);
    }

     public override IEnumerator Die()
    {
        boxCollider_Collision.isTrigger = false;
        float time = 0;
        while (time < 2)
        {
            time += .5f;
            skinnedMeshRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            skinnedMeshRenderer.enabled = true;
            yield return new WaitForSeconds(0.25f);
            //    Debug.Log(time);
        }
        //     Debug.Log("死亡確認");
        if (!photonView.IsMine && !isActive_Skill_Two)
        {

            itemManager.speed = 1;
            itemManager.bombCount = 1;
            itemManager.firePower = 2;
        }
        itemManager.isNormal = true;
        itemManager.isPenetration = false;
        itemManager.isDiffuse = false;
        itemManager.isBounce = false;
        itemManager.isKick = false;
        itemManager.isThrow = false;
        itemManager.isBarrier = false;
        itemManager.isReflection = false;
        itemManager.isJump = false;
        itemManager.heart -= 1;
        isInWall = false;
        stageUIManager.heeartText.text = itemManager.heart.ToString();
        dead = false;
    }



}
