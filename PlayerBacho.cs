using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerBacho : PlayerBase
{
    [SerializeField] GameObject TimeLine;
    [SerializeField] GameObject effect_skill_one;
    [SerializeField] GameObject effect_skill_two;
    [SerializeField] Transform effect_pos;
    private GameObject effectClone;

    
    protected override void Skill_One()
    {      
        photonView.RPC(nameof(Skill_One_Relay), RpcTarget.All, effect_pos.position, transform.rotation);
    }

    [PunRPC]
    private void Skill_One_Relay(Vector3 pos,Quaternion rot)
    {
        base.Skill_One();
        StartCoroutine(Skill_One_Corutine(pos, rot));
    }

    IEnumerator  Skill_One_Corutine(Vector3 pos,Quaternion rot)
    {
        effectClone = Instantiate(effect_skill_one, pos, rot);
        yield return new WaitForSeconds(.7f);
        Destroy(effectClone);
    }

    
    protected override void Skill_Two()
    {      
        photonView.RPC(nameof(Skill_Two_Relay), RpcTarget.All, effect_pos.position, transform.rotation);
    }

    [PunRPC]
    private void Skill_Two_Relay(Vector3 pos ,Quaternion rot)
    {
        base.Skill_Two();
        StartCoroutine(Effect_Skill_Two(pos, rot));
    }

    IEnumerator Effect_Skill_Two(Vector3 pos, Quaternion rot)
    {
        yield return new WaitForSeconds(1);
        effectClone = Instantiate(effect_skill_two, pos, rot);
        Destroy(effectClone, 0.7f);
    }
}
