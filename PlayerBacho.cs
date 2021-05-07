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

    [PunRPC]
    protected override void Skill_One()
    {
        base.Skill_One();
        effectClone = Instantiate(effect_skill_one, effect_pos.position, transform.rotation);
        Destroy(effectClone, 0.7f);
    }

    [PunRPC]
    protected override void Skill_Two()
    {
        base.Skill_Two();
        StartCoroutine(Effect_Skill_Two());
        //     GameObject timeline=  Instantiate(TimeLine);
        //       Destroy(timeline, 2.5f);
    }

    IEnumerator Effect_Skill_Two()
    {
        yield return new WaitForSeconds(1);
        effectClone = Instantiate(effect_skill_two, effect_pos.position, transform.rotation);
        Destroy(effectClone, 0.7f);
    }
}
