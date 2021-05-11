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
        photonView.RPC(nameof(Sonsaku_Skill_Two), RpcTarget.All, playerPos, shotPoint.position, angle, bombType, firePower,GManager.Instance.playerNum);
    }

    [PunRPC]
    private void Sonsaku_Skill_Two(Vector3 playerPos,Vector3 shotPos,float angle,int bombType,int firePower,int explosionNum)
    {
        base.Skill_Two();
        StartCoroutine(Bombardment(playerPos, shotPos, angle, bombType, firePower,explosionNum));
    }

    IEnumerator TrigerSwitch()
    {
        yield return new WaitForSeconds(1.8f);
        
        yield return new WaitForSeconds(0.7f);
       
    }

    IEnumerator Bombardment(Vector3 playerPos,Vector3 shotPos, float angle, int bombType, int firePower, int explosionNum)
    {
        yield return new WaitForSeconds(2f);
        exitCollision.SetActive(false);
        for (int i = -1; i < 2; i++)
        {

            if (angle == 0 || angle == 180)
            {
                GameObject bombClone = BombManager.Instance.BombInstantiate(shotPos, bombId++, actorNum, bombType, firePower, explosionNum);//, isKick);
                Bomb bombCloneSc = bombClone.GetComponent<Bomb>();
                if (angle == 0)
                {
                    bombCloneSc.isForward = true;
                }
                else if (angle == 180)
                {
                    bombCloneSc.isBack = true;
                }
                bombCloneSc.isSkipOver = true;
                bombCloneSc.angle = 1;
                bombClone.transform.DOJump(new Vector3(Mathf.CeilToInt(playerPos.x + (i * 2)), 0.5f, Mathf.CeilToInt((5 * Mathf.Cos(angle * Mathf.PI / 180) + playerPos.z))), 2, 1, 0.3f);

            }
            else if (angle == 90 || angle == 270)
            {
                GameObject bombClone = BombManager.Instance.BombInstantiate(shotPos, bombId++, actorNum, bombType, firePower, explosionNum);//, isKick);
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
                bombClone.transform.DOJump(new Vector3(Mathf.CeilToInt(playerPos.x + (5 * Mathf.Sin(angle * Mathf.PI / 180))), 0.5f, Mathf.CeilToInt(playerPos.z + (i * 2))), 2, 1, 0.3f);
            }
        }
        yield return new WaitForSeconds(.3f);
        exitCollision.SetActive(true);
    }
}
