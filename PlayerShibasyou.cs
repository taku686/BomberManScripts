using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShibasyou : PlayerBase
{
 //   [SerializeField] GameObject TimeLine;
    [SerializeField] GameObject effect_skill_one;
    //   [SerializeField] GameObject effect_skill_two;
    [SerializeField] GameObject effect_Two_Obj; //エフェクトが決まるまでの仮
    [SerializeField] Transform effect_pos;
    [SerializeField] Transform effect_pos_Skill_One;
    private GameObject effectClone;
    private GameObject effectClone_Two;

  

    [PunRPC]
    protected override void Skill_One()
    {
        base.Skill_One();
        effectClone = Instantiate(effect_skill_one,effect_pos_Skill_One.position, transform.rotation);
        StartCoroutine(Skill_One_Collider());
        Destroy(effectClone, 0.7f);
    }

    [PunRPC]
    protected override void Skill_Two()
    {
        base.Skill_Two();
        isActive_Skill_Two = true;
        StartCoroutine(Skill_Two_Activate());
        effectClone_Two = Instantiate(effect_Two_Obj, effect_pos.position, Quaternion.Euler(90, transform.rotation.eulerAngles.y + 90, 0));
        if (photonView.IsMine)
        { 
            effectClone_Two.GetComponent<Counter>().enabled = true;
        }
        effectClone_Two.transform.SetParent(effect_pos);
    }

    IEnumerator Skill_One_Collider()
    {
        yield return new WaitForSeconds(0.4f);
        effectClone.GetComponentInChildren<BoxCollider>().enabled = true;
    }

    IEnumerator Skill_Two_Activate()
    {
       
        yield return new WaitForSeconds(15);
       
        isActive_Skill_Two = false;
        Destroy(effectClone_Two);
    }
}
