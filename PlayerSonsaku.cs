using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
public class PlayerSonsaku : PlayerBase
{
    //   [SerializeField] GameObject TimeLine;
    [SerializeField] GameObject effect_skill_one;
    //   [SerializeField] GameObject effect_skill_two;
    //    [SerializeField] GameObject effect_Two_Obj; //エフェクトが決まるまでの仮
    [SerializeField] Transform effect_pos;
    private GameObject effectClone;
    [SerializeField] Transform shotPoint;
    public int actorNum;

    protected override void Start()
    {
        base.Start();
        actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    [PunRPC]
    protected override void Skill_One()
    {
        base.Skill_One();
        effectClone = Instantiate(effect_skill_one, effect_pos.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0));
        Destroy(effectClone, 0.7f);
    }

    protected override void Skill_Two()
    {
        Vector3 playerPos = transform.position;
        float angle = transform.eulerAngles.y;
        StartCoroutine(TrigerSwitch());
        int bombType = BombType();
        int firePower = itemManager.firePower;
        bool isKick = itemManager.isKick;
        photonView.RPC(nameof(Sonsaku_Skill_Two), RpcTarget.All, playerPos, angle, bombType, firePower, isKick);
    }

    [PunRPC]
    private void Sonsaku_Skill_Two(Vector3 playerPos,float angle,int bombType,int firePower,bool isKick)
    {
        isSkill_Two = true;
        animator.SetTrigger("Passive");   
        StartCoroutine(Skill_Two_Downtime());
        StartCoroutine(Bombardment(playerPos,angle,bombType,firePower,isKick));
    }

    IEnumerator TrigerSwitch()
    {
        yield return new WaitForSeconds(1.8f);
        exitCollision.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        exitCollision.SetActive(true);
    }

    IEnumerator Bombardment(Vector3 playerPos, float angle, int bombType, int firePower, bool isKick)
    {
        yield return new WaitForSeconds(2f);
        GameObject bombClone1;
        GameObject bombClone2;
        GameObject bombClone3;
        Bomb bombClone1Sc;
        Bomb bombClone2Sc;
        Bomb bombClone3Sc;

        if (angle == 0)
        {
            bombClone1 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone2 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone3 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone1.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x), 0.5f, Mathf.RoundToInt(playerPos.z + 5)), 2, 1, 0.3f);
            bombClone2.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x + 2), 0.5f, Mathf.RoundToInt(playerPos.z + 5)), 2, 1, 0.3f);
            bombClone3.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x - 2), 0.5f, Mathf.RoundToInt(playerPos.z + 5)), 2, 1, 0.3f);
            bombClone1Sc = bombClone1.GetComponent<Bomb>();
            bombClone2Sc = bombClone2.GetComponent<Bomb>();
            bombClone3Sc = bombClone3.GetComponent<Bomb>();
            bombClone1Sc.isSkipOver = true;
            bombClone2Sc.isSkipOver = true;
            bombClone3Sc.isSkipOver = true;
            bombClone1Sc.isForward = true;
            bombClone2Sc.isForward = true;
            bombClone3Sc.isForward = true;
            bombClone1Sc.angle = 1;
            bombClone2Sc.angle = 2;
            bombClone3Sc.angle = 3;
        }
        else if (angle == 90)
        {
            bombClone1 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone2 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone3 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone1.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x + 5), 0.5f, Mathf.RoundToInt(playerPos.z)), 2, 1, 0.3f);
            bombClone2.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x + 5), 0.5f, Mathf.RoundToInt(playerPos.z + 2)), 2, 1, 0.3f);
            bombClone3.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x + 5), 0.5f, Mathf.RoundToInt(playerPos.z - 2)), 2, 1, 0.3f);
            bombClone1Sc = bombClone1.GetComponent<Bomb>();
            bombClone2Sc = bombClone2.GetComponent<Bomb>();
            bombClone3Sc = bombClone3.GetComponent<Bomb>();
            bombClone1Sc.isSkipOver = true;
            bombClone2Sc.isSkipOver = true;
            bombClone3Sc.isSkipOver = true;
            bombClone1Sc.isRight = true;
            bombClone2Sc.isRight = true;
            bombClone3Sc.isRight = true;
            bombClone1Sc.angle = 1;
            bombClone2Sc.angle = 2;
            bombClone3Sc.angle = 3;
        }
        else if (angle == 180)
        {
            bombClone1 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone2 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone3 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone1.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x), 0.5f, Mathf.RoundToInt(playerPos.z - 5)), 2, 1, 0.3f);
            bombClone2.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x + 2), 0.5f, Mathf.RoundToInt(playerPos.z - 5)), 2, 1, 0.3f);
            bombClone3.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x - 2), 0.5f, Mathf.RoundToInt(playerPos.z - 5)), 2, 1, 0.3f);
            bombClone1Sc = bombClone1.GetComponent<Bomb>();
            bombClone2Sc = bombClone2.GetComponent<Bomb>();
            bombClone3Sc = bombClone3.GetComponent<Bomb>();
            bombClone1Sc.isSkipOver = true;
            bombClone2Sc.isSkipOver = true;
            bombClone3Sc.isSkipOver = true;
            bombClone1Sc.isBack = true;
            bombClone2Sc.isBack = true;
            bombClone3Sc.isBack = true;
            bombClone1Sc.angle = 1;
            bombClone2Sc.angle = 2;
            bombClone3Sc.angle = 3;
        }
        else if (angle == 270)
        {
            bombClone1 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone2 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone3 = BombManager.Instance.BombInstantiate(shotPoint.position, bombId++, actorNum, bombType, firePower, isKick);
            bombClone1.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x - 5), 0.5f, Mathf.RoundToInt(playerPos.z)), 2, 1, 0.3f);
            bombClone2.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x - 5), 0.5f, Mathf.RoundToInt(playerPos.z + 2)), 2, 1, 0.3f);
            bombClone3.transform.DOJump(new Vector3(Mathf.RoundToInt(playerPos.x - 5), 0.5f, Mathf.RoundToInt(playerPos.z - 2)), 2, 1, 0.3f);
            bombClone1Sc = bombClone1.GetComponent<Bomb>();
            bombClone2Sc = bombClone2.GetComponent<Bomb>();
            bombClone3Sc = bombClone3.GetComponent<Bomb>();
            bombClone1Sc.isSkipOver = true;
            bombClone2Sc.isSkipOver = true;
            bombClone3Sc.isSkipOver = true;
            bombClone1Sc.isLeft = true;
            bombClone2Sc.isLeft = true;
            bombClone3Sc.isLeft = true;
            bombClone1Sc.angle = 1;
            bombClone2Sc.angle = 2;
            bombClone3Sc.angle = 3;
        }
    }
}
