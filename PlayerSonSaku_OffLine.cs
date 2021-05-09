using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSonSaku_OffLine : PlayerBase_OffLine
{
    [SerializeField] GameObject effect_skill_one;
    [SerializeField] Transform effect_pos;
    private GameObject effectClone;
    [SerializeField] Transform shotPoint;
    public List<GameObject> bombClones = new List<GameObject>();
    private int actorNum = 1;
    protected override void Skill_One()
    {
        base.Skill_One();
        effectClone = Instantiate(effect_skill_one, effect_pos.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0));
        Destroy(effectClone, 0.7f);
    }

    protected override void Skill_Two()
    {
        base.Skill_Two();
        StartCoroutine(Bombardment(shotPoint.position, transform.eulerAngles.y, BombType(), itemManager.firePower, itemManager.isKick));
    }

    IEnumerator Bombardment(Vector3 shotPos, float angle, int bombType, int firePower, bool isKick)
    {
        yield return new WaitForSeconds(2f);
        exitCollision.enabled = false;
        //   bombClones.Clear();
        for (int i = -1; i < 2; i++)
        {
            //  Debug.Log(i + 1 + "回目");
            if (angle == 0 || angle == 180)
            {
                GameObject bombClone = BombManager.Instance.BombInstantiate(shotPos, bombId++, actorNum, bombType, firePower);//, isKick);
                Bomb bombCloneSc = bombClone.GetComponent<Bomb>();
                //     Debug.Log("Cos" + Mathf.Cos(angle * Mathf.PI / 180));
                //     Debug.Log("angle" + angle);
                if (angle == 0)
                {
                    //       Debug.Log("GetComponentBombScript");
                    bombCloneSc.isForward = true;
                }
                else if (angle == 180)
                {
                    bombCloneSc.isBack = true;
                }
                bombCloneSc.isSkipOver = true;
                bombCloneSc.angle = 1;
                bombClone.transform.DOJump(new Vector3(Mathf.CeilToInt(transform.position.x + (i * 2)), 0.5f, Mathf.CeilToInt((5 * Mathf.Cos(angle * Mathf.PI / 180) + transform.position.z))), 2, 1, 0.3f);

            }
            else if (angle == 90 || angle == 270)
            {
                GameObject bombClone = BombManager.Instance.BombInstantiate(shotPos, bombId++, actorNum, bombType, firePower);//, isKick);
                Bomb bombCloneSc = bombClone.GetComponent<Bomb>();
                if (angle == 90)
                {
                    bombCloneSc.isRight = true;
                }
                else if (angle == 270)
                {
                    bombCloneSc.isLeft = true;
                }
                bombCloneSc.isSkipOver = true;
                bombCloneSc.angle = 1;
                bombClone.transform.DOJump(new Vector3(Mathf.CeilToInt(transform.position.x + (5 * Mathf.Sin(angle * Mathf.PI / 180))), 0.5f, Mathf.CeilToInt(transform.position.z + (i * 2))), 2, 1, 0.3f);
            }
        }
        yield return new WaitForSeconds(.3f);
        exitCollision.enabled = true;
    }
}
