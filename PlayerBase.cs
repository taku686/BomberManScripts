/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using System.Collections;
using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using DG.Tweening;
using Chronos;

public class PlayerBase : MonoBehaviourPunCallbacks
{
    public float moveSpeed;
    public bool canDropBombs = true;
    //Can the player drop bombs?
    public bool canMove = true;
    //Can the player move?
    public bool dead = false;
    private GameObject shield;

    //Cached components
    protected Rigidbody rigidBody;
    protected Transform myTransform;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected StageUIManager stageUIManager;
    public ItemManager itemManager;
    private Quaternion initRotation;
    protected bool isSkill_One;
    public bool isSkill_Two;
    public bool isActive_Skill_Two;
    [SerializeField] protected float WaitTime_SkillOne;
    [SerializeField] protected float WaitTime_SkillTwo;
    protected bool isInWall;
    [SerializeField]protected LocalClock localClock;
    [SerializeField] protected Timeline timeline;
    [SerializeField] LayerMask layerMask;
    public static bool isHold;
    public static bool isThrowing;
    private GameObject m_bomb;
    private Bomb m_bombSc;
//    private bool isTouchBomb;
    [SerializeField] Transform bombPos;
    protected GlobalClock[] globalClock;
    [SerializeField] protected bool isSkill_One_Rpc;
    [SerializeField] protected bool isSkill_Two_Rpc;
    protected int bombId = 0;
    [SerializeField]protected GameObject exitCollision;
    [SerializeField]protected BoxCollider boxCollider_Collision;
    public bool isDead;
    protected int downTime = 0;
    private bool isThrowWait = true;
    private bool isHoldWait = true;
    protected BattleManager sc_battleManager;
    public int int_playerNum;

    // Use this for initialization
    protected virtual void Start()
    {
   //          Debug.Log("LocalPlayer" + PhotonNetwork.LocalPlayer.ActorNumber);
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        photonView.RPC(nameof(Initialized), RpcTarget.All);
    }

    [PunRPC]
    protected void Initialized()
    {
        Debug.Log("初期動作");
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        stageUIManager = GameObject.Find("UIManager").GetComponent<StageUIManager>();
        sc_battleManager = GameObject.Find("GameManager").GetComponent<BattleManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (WarpGrid.isEnableWarp && photonView.IsMine&&!isInWall)
        {
            UpdateMovement();

            if (Input.GetKeyDown(KeyCode.J) && !isSkill_One && isSkill_One_Rpc)
            {
                photonView.RPC(nameof(Skill_One), RpcTarget.All);
            }
            else if (Input.GetKeyDown(KeyCode.J) && !isSkill_One && !isSkill_One_Rpc)
            {
                Skill_One();
            }          
            if (Input.GetKeyDown(KeyCode.L) && !isSkill_Two && !isActive_Skill_Two && isSkill_Two_Rpc)
            {
                photonView.RPC(nameof(Skill_Two), RpcTarget.All);
            }
            else if (Input.GetKeyDown(KeyCode.L) && !isSkill_Two && !isActive_Skill_Two && !isSkill_Two_Rpc)
            {
                Skill_Two();
            }
            if (Input.GetKeyDown(KeyCode.H) && !isHold && itemManager.isThrow&&isHoldWait )
            {
                isHoldWait = false;
                StartCoroutine(HoldWait_Corutine());
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, .7f, layerMask))
                {
                    if (hit.collider.CompareTag("Bomb"))
                    {
                        hit.collider.GetComponent<SphereCollider>().enabled = false;                  
                        Bomb bombSc = hit.transform.GetComponent<Bomb>();
                        photonView.RPC(nameof(Lift), RpcTarget.All,hit.transform.position ,bombSc.Id, bombSc.OwnerId,bombSc.BombType,bombSc.m_firePower,GManager.Instance.playerNum);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.H) && isHold&&!isThrowWait )
            {
                isThrowWait = true;
                float angle = (transform.rotation.eulerAngles.y) % 360;
                Vector3 playerPos = transform.position;
                photonView.RPC(nameof(Throw), RpcTarget.All, angle, playerPos);
            }
            if(Input.GetKeyDown(KeyCode.K) && itemManager.isKick)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, .7f, layerMask))
                {
                    if (hit.collider.CompareTag("Bomb"))
                    {
                        Bomb bombSc = hit.transform.GetComponent<Bomb>();
                        photonView.RPC(nameof(Kick), RpcTarget.All, bombSc.Id, bombSc.OwnerId);
                    }
                }
            }

            if (isDead)
            {
                isDead = false;
                photonView.RPC(nameof(Die_Player), RpcTarget.All,int_playerNum);
            }
            
                   
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Mathf.Abs(h) > 0.001f)
                v = 0;

            var speed = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            animator.SetFloat("speedv", speed);
        }
    }

    [PunRPC]
    protected void Kick(int id ,int ownerId)
    {
        if(BombManager.Instance.BombSearch(id,ownerId)!= null)
        {
            BombManager.Instance.BombSearch(id, ownerId).GetComponent<Bomb>().m_isKick = true;
        }
        else
        {
            return;
        }
    }
   
    protected virtual void Skill_One()
    {
        isSkill_One = true;
        animator.SetTrigger("Attack");
        StartCoroutine(Skill_One_Downtime());
    }
   
    protected virtual void Skill_Two()
    {
        isSkill_Two = true;
        animator.SetTrigger("Passive");
        StartCoroutine(Skill_Two_Downtime());
    }

    protected IEnumerator Skill_One_Downtime()
    {
        yield return new WaitForSeconds(WaitTime_SkillOne);
        isSkill_One = false;
    }

    protected IEnumerator Skill_Two_Downtime()
    {
        yield return new WaitForSeconds(WaitTime_SkillTwo);
        isSkill_Two = false;
    }

    [PunRPC]
    protected virtual void Lift(Vector3 pos,int id, int ownerId,int bombType,int fireType,int explosionNum)
    {
        animator.SetTrigger("Throw");
        GameObject bomb;
        if (BombManager.Instance.BombSearch(id, ownerId) != null)
        {
             bomb = BombManager.Instance.BombSearch(id, ownerId);
        }
        else
        {
            bomb = BombManager.Instance.BombInstantiate(pos, id, ownerId, bombType, fireType,explosionNum);
        }
        
        m_bomb = bomb;
        m_bombSc = bomb.GetComponent<Bomb>();
        m_bombSc.isBombWait = true;
        bomb.transform.SetParent(bombPos);
        bomb.transform.localPosition = Vector3.zero;
        isHold = true;
        StartCoroutine(HoldUp());
    }

    IEnumerator HoldUp()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Hold", true);
        yield return new WaitForSeconds(.5f);
        isThrowWait = false;
    }

    [PunRPC]
    protected virtual void Throw(float angle, Vector3 playerPos)
    {
        animator.SetTrigger("Throwing");
        animator.SetBool("Hold", false);
        m_bomb.transform.parent = null;
        m_bomb.GetComponent<SphereCollider>().enabled = true;
        if (photonView.IsMine)
        {
            StartCoroutine(ExitCollisionSwitch_Corutine());
        }
        m_bombSc.angle = angle;
        m_bombSc.ThrowingBall(angle, playerPos);
        isThrowing = true;
        isHold = false;
        m_bombSc = null;
        m_bomb = null;
    }

    IEnumerator ExitCollisionSwitch_Corutine()
    {
        exitCollision.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        exitCollision.SetActive(true);
    }

    private void UpdateMovement ()
    {
        if (!canMove)
        { //Return if player can't move
            return;
        }
        //Depending on the player number, use different input for moving     
        UpdatePlayerMovement();        
    }

    protected virtual void UpdatePlayerMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !isSkill_One && !isSkill_Two)
        { //Up movement
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, transform.position.z);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed + ((itemManager.speed - 1) * 3 / 7)) * localClock.localTimeScale * timeline.lastTimeScale;
            myTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isSkill_One && !isSkill_Two)
        { //Left movement
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.RoundToInt(transform.position.z));
            rigidBody.velocity = new Vector3(-moveSpeed - ((itemManager.speed - 1) * 3 / 7), rigidBody.velocity.y, rigidBody.velocity.z) * localClock.localTimeScale * timeline.lastTimeScale;
            myTransform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isSkill_One && !isSkill_Two)
        { //Down movement
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, transform.position.z);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed - ((itemManager.speed - 1) * 3 / 7)) * localClock.localTimeScale * timeline.lastTimeScale;
            myTransform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !isSkill_One && !isSkill_Two)
        { //Right movement
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.RoundToInt(transform.position.z));
            rigidBody.velocity = new Vector3(moveSpeed + ((itemManager.speed - 1) * 3 / 7), rigidBody.velocity.y, rigidBody.velocity.z) * localClock.localTimeScale * timeline.lastTimeScale;
            myTransform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }

        if (canDropBombs && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space)) && !dead && !isHold && !isSkill_One && !isSkill_Two && itemManager.bombCount >= BombManager.Instance.bombNumOnStage && downTime == 0)
        {
            StartCoroutine(DropBomb_DownTime());
            int bombType = BombType();
            int firePower = itemManager.firePower;
            bool isKick = itemManager.isKick;
            var pos = new Vector3
(
    Mathf.RoundToInt(myTransform.position.x),
   0.4f,
    Mathf.RoundToInt(myTransform.position.z)
);
            photonView.RPC(nameof(DropBomb), RpcTarget.All, pos, bombId++, bombType, firePower, GManager.Instance.playerNum);
        }
    }

    protected IEnumerator DropBomb_DownTime()
    {
        downTime = 1;
        yield return new WaitForSeconds(.2f);
        downTime = 0;
    }

    IEnumerator HoldWait_Corutine()
    {
        yield return new WaitForSeconds(1);
        isHoldWait = true;
    }

    [PunRPC]
    /// <summary>
    /// Drops a bomb beneath the player
    /// </summary>
    protected void DropBomb(Vector3 bombPos,int bombId,int bombType,int firePower,int explosionNum,PhotonMessageInfo info)
    {
        BombManager.Instance.BombInstantiate(bombPos, bombId, info.Sender.ActorNumber, bombType, firePower,explosionNum);
    }

    protected int BombType()
    {
        if (itemManager.isNormal)
        {
            return 1;
        }
        else if (itemManager.isPenetration)
        {
            return 2;
        }
        else if (itemManager.isDiffuse)
        {
            return 3;
        }
        else if (itemManager.isBounce)
        {
            return 4;
        }
        else
        {
            return 0;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //     Debug.Log("侵入検知");
      
        if (other.CompareTag("PenetrationBomb"))
        {
            Destroy(other.gameObject);
            itemManager.isNormal = false;
            itemManager.isPenetration = true;
            itemManager.isDiffuse = false;
            itemManager.isBounce = false;
        }
        else if (other.CompareTag("DiffuseBomb"))
        {
            Destroy(other.gameObject);
            itemManager.isNormal = false;
            itemManager.isPenetration = false;
            itemManager.isDiffuse = true;
            itemManager.isBounce = false;
        }
        else if (other.CompareTag("BounceBomb"))
        {
            Destroy(other.gameObject);
            itemManager.isNormal = false;
            itemManager.isPenetration = false;
            itemManager.isDiffuse = false;
            itemManager.isBounce = true;
        }
        else if (other.CompareTag("Bomb"))
        {
            //    Debug.Log("ボム接触" + isTouchBomb);
      //      isTouchBomb = true;
      //      m_bomb = other.gameObject;
        }
        else if (other.CompareTag("FreezeEffect"))
        {
            StartCoroutine(FreezeMove());
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bomb") && !isHold)
        {
            //    Debug.Log("ボム接触");
     //       isTouchBomb = false;
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        Vector3 dir = collision.transform.localPosition - transform.position;
        if ((collision.collider.CompareTag("BreakingWall") || collision.collider.CompareTag("Wall") || collision.collider.CompareTag("OutWall")) && dir.sqrMagnitude < 0.3f && !isInWall && !isActive_Skill_Two)
        {
            isInWall = true;
            Debug.Log("死亡確認");
            boxCollider_Collision.isTrigger = true;
            if (GManager.Instance.playerNum == 1)
            {
                transform.position = new Vector3(-1, 0, 10);
            }
            else if (GManager.Instance.playerNum == 2)
            {
                transform.position = new Vector3(10, 0, 10);
            }
            else if (GManager.Instance.playerNum == 3)
            {
                transform.position = new Vector3(-1, 0, 0);
            }
            else if (GManager.Instance.playerNum == 4)
            {
                transform.position = new Vector3(10, 0, 0);
            }
            photonView.RPC(nameof(Die_Player), RpcTarget.All,0);
        }
    }

    IEnumerator FreezeMove()
    {
        localClock.localTimeScale = 0;
        yield return new WaitForSeconds(2);
        localClock.localTimeScale = 1;
    }

    [PunRPC]
    protected  void Die_Player(int playerNum)
    {
        StartCoroutine(Die(playerNum));
    }

    public virtual IEnumerator Die(int playerNum)
    {
        boxCollider_Collision.isTrigger = false;
        if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.TimeMode)
        {
            if (playerNum == 1)
            {
                sc_battleManager.UpdateScore(10, 1);
            }
            else if (playerNum == 2)
            {
                sc_battleManager.UpdateScore(10, 2);
            }
            else if (playerNum == 3)
            {
                sc_battleManager.UpdateScore(10, 3);
            }
            else if (playerNum == 4)
            {
                sc_battleManager.UpdateScore(10, 4);
            }
            else
            {
                sc_battleManager.UpdateScore(0, 0);
            }
        }
        else if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.SurvivalMode)
        {
            if (GManager.Instance.playerNum == 1)
            {
                sc_battleManager.UpdateScore(-1, 1);
            }
            else if (GManager.Instance.playerNum == 2)
            {
                sc_battleManager.UpdateScore(-1, 2);
            }
            else if (GManager.Instance.playerNum == 3)
            {
                sc_battleManager.UpdateScore(-1, 3);
            }
            else if (GManager.Instance.playerNum == 4)
            {
                sc_battleManager.UpdateScore(-1, 4);
            }
        }
        
        float time = 0;
        while (time < 2)
        {
            time += .5f;
            skinnedMeshRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            skinnedMeshRenderer.enabled = true;
            yield return new WaitForSeconds(0.25f);
            //    Debug.Log(time);
        }
        //     Debug.Log("死亡確認");
        if (photonView.IsMine)
        {
            isInWall = false;
            itemManager.heart -= 1;
            itemManager.speed = 1;
            itemManager.bombCount = 1;
            itemManager.firePower = 2;
            itemManager.isNormal = true;
            itemManager.isPenetration = false;
            itemManager.isDiffuse = false;
            itemManager.isBounce = false;
            itemManager.isKick = false;
            itemManager.isThrow = false;
            itemManager.isBarrier = false;
            itemManager.isReflection = false;
            itemManager.isJump = false;
            stageUIManager.heeartText.text = itemManager.heart.ToString();
            dead = false;
        }
    }

    private void OnDrawGizmos()
    {
        /*
        Vector3 dir = Vector3.zero;
        if (transform.rotation.eulerAngles.y == 0)
        {
            dir = Vector3.forward;
        }
        else if (transform.rotation.eulerAngles.y == 90)
        {
            dir = Vector3.right;
        }
        else if (transform.rotation.eulerAngles.y == 180)
        {
            dir = Vector3.back;
        }
        else if (transform.rotation.eulerAngles.y == 270)
        {
            dir = Vector3.left;
        }
        Debug.DrawRay(new Vector3(transform.position.x, 0.6f, transform.position.z), dir, Color.blue, .1f);
        */
    }
}
