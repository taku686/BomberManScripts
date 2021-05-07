using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSyokatsuryou_OffLine : PlayerBase_OffLine
{
    [SerializeField] GameObject effect_skill_one;
    [SerializeField] GameObject effect_skill_two;
    [SerializeField] Transform effect_pos;
    private GameObject effectClone;
    private GameObject effectClone_Two;
   
    protected override void Skill_One()
    {
        base.Skill_One();     
        StartCoroutine(Skill_One_Active());
        effectClone = Instantiate(effect_skill_one, effect_pos.position, transform.rotation);
        Destroy(effectClone, 0.7f);
    }

   
    protected override void Skill_Two()
    {
        base.Skill_Two();
        isActive_Skill_Two = true;
        StartCoroutine(Skill_Two_Activate());
        effectClone_Two = Instantiate(effect_skill_two, effect_pos.position, transform.rotation, effect_pos);
    }



    IEnumerator Skill_One_Active()
    {
        exitCollision.enabled = false;
        yield return new WaitForSeconds(0.7f);
        exitCollision.enabled = true;
    }

    IEnumerator Skill_Two_Activate()
    {
        yield return new WaitForSeconds(10);
        isActive_Skill_Two = false;
        Destroy(effectClone_Two);
    }
}
