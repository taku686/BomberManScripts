using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class BombManager : SingletonMonoBehaviour<BombManager>
{
    private List<GameObject> bombClones = new List<GameObject>();
    [SerializeField] GameObject normalBomb;
    [SerializeField] GameObject penetrationBomb;
    [SerializeField] GameObject diffuseBomb;
    [SerializeField] GameObject bounceBomb;
    [SerializeField] GameObject obj_player1ExplosionEffect;
    [SerializeField] GameObject obj_player2ExplosionEffect;
    [SerializeField] GameObject obj_player3ExplosionEffect;
    [SerializeField] GameObject obj_player4ExplosionEffect;
    [SerializeField] ItemManager itemManager;
    public int bombNumOnStage = 1;
    protected override void Awake()
    {
        base.Awake();
    }

    public void BombConsumptionCount(int num)
    {
        bombNumOnStage += num;
    }


    public GameObject BombInstantiate(Vector3 bombPos, int id, int playerActorId, int bombType, int firePower, int explosionNum)//, bool isKick)
    {
        GameObject bombClone;
        Bomb bombSc;
        if (playerActorId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            BombConsumptionCount(1);
        }
        if (bombType == 1)
        {
            bombClone = Instantiate(normalBomb, bombPos, normalBomb.transform.rotation);
            bombClones.Add(bombClone);
            bombClone.GetComponent<SphereCollider>().enabled = true;
            bombSc = bombClone.GetComponent<Bomb>();
            bombSc.Initialized(id, playerActorId, firePower, bombType, explosionNum);//,isKick);
            return bombClone;
        }
        else if (bombType == 2)
        {
            bombClone = Instantiate(penetrationBomb, bombPos, penetrationBomb.transform.rotation);
            bombClones.Add(bombClone);
            bombClone.GetComponent<SphereCollider>().enabled = true;
            bombSc = bombClone.GetComponent<Bomb>();
            bombSc.Initialized(id, playerActorId, firePower, bombType, explosionNum);//, isKick);
            return bombClone;
        }
        else if (bombType == 3)
        {
            bombClone = Instantiate(diffuseBomb, bombPos, diffuseBomb.transform.rotation);
            bombClones.Add(bombClone);
            bombClone.GetComponent<SphereCollider>().enabled = true;
            bombSc = bombClone.GetComponent<Bomb>();
            bombSc.Initialized(id, playerActorId, firePower, bombType, explosionNum);//, isKick);
            return bombClone;
        }
        else if (bombType == 4)
        {
            bombClone = Instantiate(bounceBomb, bombPos, bounceBomb.transform.rotation);
            bombClones.Add(bombClone);
            bombClone.GetComponent<SphereCollider>().enabled = true;
            bombSc = bombClone.GetComponent<Bomb>();
            bombSc.Initialized(id, playerActorId, firePower, bombType, explosionNum);//, isKick);
            return bombClone;
        }
        else
        {
            return null;
        }
    }

    public void BombRemove(int id,int ownerId)
    {
        bombClones.Remove(BombSearch(id, ownerId));
    }

    public GameObject BombSearch(int id, int ownerId)
    {
        foreach (var bombClone in bombClones.Where(b => b.GetComponent<Bomb>().Equals(id, ownerId)))
        {
        //    Debug.Log("目的のボムを見つけました");
            return bombClone;
        }
       // Debug.Log("ボムは見つかりませんでした");
        return null;
    }

    public GameObject InstantiateExplosionEffect(Vector3 pos,int playerNum)
    {
        if(playerNum == 1)
        {
            return Instantiate(obj_player1ExplosionEffect, pos, obj_player1ExplosionEffect.transform.rotation);
        }
        else if (playerNum == 2)
        {
            return Instantiate(obj_player2ExplosionEffect, pos, obj_player2ExplosionEffect.transform.rotation);
        }
        else if (playerNum == 3)
        {
            return Instantiate(obj_player3ExplosionEffect, pos, obj_player3ExplosionEffect.transform.rotation);
        }
        else if (playerNum == 4)
        {
            return Instantiate(obj_player4ExplosionEffect, pos, obj_player4ExplosionEffect.transform.rotation);
        }
        else
        {
            return null;
        }
    }
}
