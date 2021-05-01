using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Photon.Pun;

public class PlayerRyofu : PlayerBase
{
    //   [SerializeField] GameObject TimeLine;
    [SerializeField] GameObject effect_skill_one;
    [SerializeField] GameObject effect_skill_two;
    [SerializeField] Transform effect_pos;
    [SerializeField] GameObject explosion_Effect;
    private GameObject effectClone;
    [SerializeField] LayerMask levelMask;
     List<Vector3> fallPos = new List<Vector3>();
     List<Vector3> allPos = new List<Vector3>();
    //    private Vector3[] vector3s = new Vector3[5];
    private int actorNum;

    protected override void Start()
    {
        base.Start();
        actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
    }
    [PunRPC]
    protected override void Skill_One()
    {
        base.Skill_One();     
        StartCoroutine(Skill_One_Active());
        effectClone =Instantiate(effect_skill_one, effect_pos.position, transform.rotation);
        Destroy(effectClone, 0.7f);
    }

    [PunRPC]
    protected override void Skill_Two()
    {
        base.Skill_Two();
        //        GameObject timeline = Instantiate(TimeLine);
        StartCoroutine(Meteorite());
        Destroy(timeline, 2.5f);
    }

    IEnumerator Skill_One_Active()
    {
        exitCollision.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        exitCollision.SetActive(true);
    }

    IEnumerator Meteorite()
    {
        if (photonView.IsMine)
        {
            yield return new WaitForSeconds(1.5f);
            fallPos.Clear();
            allPos.Clear();
            RaycastHit[] hits = Physics.BoxCastAll(new Vector3(4.5f, 0, 5f), new Vector3(6, 1, 5.5f), Vector3.up, Quaternion.identity, 1, levelMask);
            BombCreateData bombCreateData = new BombCreateData();
            bombCreateData.vector3s = new List<Vector3>();
            for (int i = -1; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    allPos.Add(new Vector3(i, 0.5f, j));
                    foreach (var hit in hits.Where(h => h.transform.CompareTag("Wall") || h.transform.CompareTag("BreakingWall")))
                    {
                        if (new Vector3(i, 0.5f, j) == hit.transform.position)
                        {
                            //  Debug.Log("一致");
                            fallPos.Add(new Vector3(i, 0.5f, j));
                        }
                    }
                }
            }
            for (int i = 0; i < allPos.Count; i++)
            {
                for (int j = 0; j < fallPos.Count; j++)
                {
                    if (allPos[i] == fallPos[j])
                    {
                        allPos.RemoveAt(i);
                    }
                }
            }
            for (int k = 0; k < 5; k++)
            {
                int num = Random.Range(0, allPos.Count-1);
                bombCreateData.vector3s.Add(allPos[num]);
                allPos.RemoveAt(num);
            }
            bombCreateData.bombType = BombType();
            bombCreateData.firePower = itemManager.firePower;
            bombCreateData.isKick = itemManager.isKick;
            string strData = JsonUtility.ToJson(bombCreateData);
            photonView.RPC(nameof(BombCreate), RpcTarget.All, strData);
        }
    }

    [System.Serializable]
    public struct BombCreateData
    {
        public List<Vector3> vector3s;
        public int bombType;
        public int firePower;
        public bool isKick;
    }

    [PunRPC]
    private void BombCreate(string targetPoses)
    {
        BombCreateData bombCreateData = JsonUtility.FromJson<BombCreateData>(targetPoses);

        foreach (var targetPos in bombCreateData.vector3s)
        {
            GameObject bombClone = BombManager.Instance.BombInstantiate(new Vector3(targetPos.x + 2, 8, targetPos.z), bombId++, actorNum, bombCreateData.bombType, bombCreateData.firePower, bombCreateData.isKick);
            GameObject effect = Instantiate(effect_skill_two, new Vector3(bombClone.transform.position.x, bombClone.transform.position.y + 0.5f, bombClone.transform.position.z), Quaternion.identity);
            effect.transform.SetParent(bombClone.transform);
            Destroy(effect, 1.2f);
            bombClone.transform.DOLocalMove(targetPos, 0.8f).OnComplete(() =>
            {
                GameObject effect2 = Instantiate(explosion_Effect, targetPos, explosion_Effect.transform.rotation);
                StartCoroutine(CollisionOn(effect2));
            });
        }
    }

    IEnumerator CollisionOn(GameObject effect)
    {
        Debug.Log("コライダーオン");
        yield return new WaitForSeconds(0.5f);
        effect.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnDrawGizmos()
    {
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireCube(new Vector3(4.5f, 0, 5f), new Vector3(6, 1, 5.5f) * 2);
    }
}
