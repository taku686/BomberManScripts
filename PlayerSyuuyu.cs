using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Chronos;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSyuuyu : PlayerBase
{

    [SerializeField] GameObject effect_skill_two;
    [SerializeField] TrailsFX.TrailEffect trailEffect;
    [SerializeField] JumpCollider jumpCollider;
    private GameObject effectClone;


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
        GameObject effectClone = Instantiate(effect_skill_two, transform.position, Quaternion.identity);
        if (globalClock == null)
        {
            globalClock = GameObject.Find("Timekeeper").GetComponents<GlobalClock>();
        }
        Destroy(effectClone, 2.5f);
        StartCoroutine(Skill_Two_Activate());

        //     GameObject timeline = Instantiate(TimeLine);
        //     Destroy(timeline, 2.5f);
    }
   
    IEnumerator Skill_Two_Activate()
    {
        globalClock[0].localTimeScale = 0.3f;
        timeline.globalClockKey = "Syuuyu";
        yield return new WaitForSeconds(20);
        timeline.globalClockKey = "Root";
        globalClock[0].localTimeScale = 1;
        moveSpeed = 5;
        isActive_Skill_Two = false;
       
    }


}
