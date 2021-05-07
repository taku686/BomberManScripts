using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class PlayerSyokatsuryou : PlayerBase
{
    //   [SerializeField] GameObject TimeLine;
    [SerializeField] GameObject effect_skill_one;
    [SerializeField] GameObject effect_skill_two;
    [SerializeField] Transform effect_pos;
    private GameObject effectClone;
    private GameObject effectClone_Two;
    [PunRPC]
    protected override void Skill_One()
    {
        base.Skill_One();
        exitCollision.SetActive(false);
        StartCoroutine(Skill_One_Active());
        effectClone = Instantiate(effect_skill_one, effect_pos.position, transform.rotation);
        Destroy(effectClone, 0.7f);
    }

    [PunRPC]
    protected override void Skill_Two()
    {
        base.Skill_Two();
        if (photonView.IsMine)
        {
            isActive_Skill_Two = true;           
        }
        StartCoroutine(Skill_Two_Activate());
        effectClone_Two = Instantiate(effect_skill_two, effect_pos.position, transform.rotation);
        effectClone_Two.transform.SetParent(effect_pos);
    }

    IEnumerator Skill_One_Active()
    {
        yield return new WaitForSeconds(0.7f);
        if (photonView.IsMine)
        {
            exitCollision.SetActive(true);
        }
    }

    IEnumerator Skill_Two_Activate()
    {

        yield return new WaitForSeconds(10);
        isActive_Skill_Two = false;
        Destroy(effectClone_Two);
    }
}
