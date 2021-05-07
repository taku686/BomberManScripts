using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class PlayerLiube : PlayerBase
{
    [SerializeField] GameObject TimeLine;
    [SerializeField] GameObject effect_skill_two;
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
    //    GameObject timeline = Instantiate(TimeLine);
    //    Destroy(timeline, 2.5f);
    }

    IEnumerator Skill_Two_Activate()
    {
        yield return new WaitForSeconds(1.8f);
        moveSpeed = 10;
        boxCollider.isTrigger = true;
        GameObject effect = Instantiate(effect_skill_two, effect_Pos2.position, effect_skill_two.transform.rotation, effect_Pos2);
        yield return new WaitForSeconds(20);
        Destroy(effect);
        moveSpeed = 5;
        isActive_Skill_Two = false;
        boxCollider.isTrigger = false;
    }
  
    protected override void UpdatePlayerMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !isSkill_One && !isSkill_Two)
        { //Up movement
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, transform.position.z);
            if (transform.position.z < 10)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed + ((itemManager.speed - 1) * 3 / 7)) * localClock.localTimeScale * timeline.lastTimeScale;
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
            }
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isSkill_One && !isSkill_Two)
        { //Left movement
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.RoundToInt(transform.position.z));
            if (transform.position.x > -1)
            {
                rigidBody.velocity = new Vector3(-moveSpeed - ((itemManager.speed - 1) * 3 / 7), rigidBody.velocity.y, rigidBody.velocity.z) * localClock.localTimeScale * timeline.lastTimeScale;
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
            }
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isSkill_One && !isSkill_Two)
        { //Down movement
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, transform.position.z);
            if (transform.position.z > 0)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed - ((itemManager.speed - 1) * 3 / 7)) * localClock.localTimeScale * timeline.lastTimeScale;
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
            }
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !isSkill_One && !isSkill_Two)
        { //Right movement
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.RoundToInt(transform.position.z));
            if (transform.position.x < 10)
            {
                rigidBody.velocity = new Vector3(moveSpeed + ((itemManager.speed - 1) * 3 / 7), rigidBody.velocity.y, rigidBody.velocity.z) * localClock.localTimeScale * timeline.lastTimeScale;
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
            }
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }

        if (canDropBombs && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space)) && !dead && !isHold && !isSkill_One && !isSkill_Two && itemManager.bombCount >= BombManager.Instance.bombNumOnStage && downTime == 0)
        {
            StartCoroutine(DropBomb_DownTime());
            int bombType = BombType();
            int firePower = itemManager.firePower;
            bool isKick = itemManager.isKick;
            var pos = new Vector3
(
    Mathf.RoundToInt(myTransform.position.x),
   0.4f,
    Mathf.RoundToInt(myTransform.position.z)
);

            photonView.RPC(nameof(DropBomb), RpcTarget.All, pos, bombId++, bombType, firePower, isKick);
        }
    }
}
