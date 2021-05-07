using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTyouun_OffLine : PlayerBase_OffLine
{
    [SerializeField] GameObject effect_skill_one;
    [SerializeField] GameObject effect_Two_Obj;
    [SerializeField] Transform effect_pos;
    private GameObject effectClone;

    protected override void Skill_One()
    {
        base.Skill_One();
        effectClone = Instantiate(effect_skill_one, effect_pos.position, transform.rotation);
        StartCoroutine(Skill_One_Collider());
        Destroy(effectClone, 0.7f);
    }

    protected override void Skill_Two()
    {
        base.Skill_Two();
        StartCoroutine(Skill_Two_Effect(transform.rotation.eulerAngles.y));
    }

    IEnumerator Skill_One_Collider()
    {
        yield return new WaitForSeconds(0.5f);
        effectClone.GetComponentInChildren<BoxCollider>().enabled = true;
    }

    IEnumerator Skill_Two_Effect(float angle)
    {
        yield return new WaitForSeconds(1.15f);
        RFX4_RaycastCollision.angle = angle;
        GameObject effect = Instantiate(effect_Two_Obj, effect_pos.position, Quaternion.Euler(0, angle - 90, 0));
        Destroy(effect, 1);
    }
}
