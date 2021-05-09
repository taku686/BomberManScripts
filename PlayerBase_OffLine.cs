using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class PlayerBase_OffLine : MonoBehaviour
{
    public float moveSpeed;
    public bool canDropBombs = true; 
    public bool canMove = true;  
    public bool dead = false;
    public ItemManager itemManager;
    public bool isSkill_Two;
    public bool isActive_Skill_Two;
    public static bool isHold;
    public static bool isThrowing;
    public bool isDead;
    protected Rigidbody rigidBody;
    protected Transform myTransform; 
    protected StageUIManager stageUIManager;   
    protected bool isSkill_One;
    protected bool isInWall;
    protected GlobalClock[] globalClock;
    protected int bombId = 0;
    protected int downTime = 0;
    [SerializeField] protected float WaitTime_SkillOne;
    [SerializeField] protected float WaitTime_SkillTwo;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;  
    [SerializeField] protected LocalClock localClock;
    [SerializeField] protected Timeline timeline;
    [SerializeField] protected BoxCollider exitCollision;
    [SerializeField] protected BoxCollider boxCollider_Collision;
    [SerializeField] LayerMask layerMask;   
    [SerializeField] Transform bombPos;
    private GameObject m_bomb;
    private Bomb m_bombSc;
    private bool isTouchBomb;
    private bool isThrowWait = true;
    private bool isHoldWait= true;
    public int PlayerOwnerId { get; private set; }

    // Use this for initialization
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        stageUIManager = GameObject.Find("UIManager").GetComponent<StageUIManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (WarpGrid.isEnableWarp && !isInWall)
        {
            UpdateMovement();

            if (Input.GetKeyDown(KeyCode.J) && !isSkill_One)
            {
                Skill_One();
            }
            if (Input.GetKeyDown(KeyCode.L) && !isSkill_Two && !isActive_Skill_Two)
            {
                Skill_Two();
            }
            if (Input.GetKeyDown(KeyCode.H) && !isHold && itemManager.isThrow&&isHoldWait )
            {
                isHoldWait = false;
                StartCoroutine(HoldWait_Corutine());
                if (Physics.Raycast(transform.position,transform.forward,out RaycastHit   hit, .7f,layerMask))
                {
                    if (hit.collider.CompareTag("Bomb"))
                    {                     
                        hit.collider.GetComponent<SphereCollider>().enabled = false;
                        m_bomb = hit.collider.gameObject;
                        Bomb bombSc = hit.transform.GetComponent<Bomb>();
                        Lift(bombSc.Id, bombSc.OwnerId);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.H) && isHold&&!isThrowWait)
            {
                isThrowWait = true;
                float angle = (transform.rotation.eulerAngles.y) % 360;
                Vector3 playerPos = transform.position;
                Throw(angle, playerPos);
            }
            if (Input.GetKeyDown(KeyCode.K) && itemManager.isKick)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, .7f, layerMask))
                {
                    if (hit.collider.CompareTag("Bomb"))
                    {
                        hit.transform.GetComponent<Bomb>().m_isKick = true;                     
                    }
                }
            }
            if (isDead)
            {
                isDead = false;
                StartCoroutine(Die());
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Mathf.Abs(h) > 0.001f)
                v = 0;

            var speed = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            animator.SetFloat("speedv", speed);
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

    protected virtual void Lift(int id, int ownerId)
    {
        animator.SetTrigger("Throw");
        GameObject bomb = BombManager.Instance.BombSearch(id, ownerId);
        m_bombSc = bomb.GetComponent<Bomb>();
        m_bombSc.isBombWait = true;
        isHold = true;
        bomb.transform.SetParent(bombPos);
        bomb.transform.localPosition = Vector3.zero;
        StartCoroutine(HoldUp());
    }

    IEnumerator HoldUp()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Hold", true);
        yield return new WaitForSeconds(.5f);
        isThrowWait = false;
    }

    protected void Throw(float angle, Vector3 playerPos)
    {
        animator.SetTrigger("Throwing");
        animator.SetBool("Hold", false);       
        m_bomb.transform.parent = null;
        StartCoroutine(ExitCollisionSwitch_Corutine());
        m_bomb.GetComponent<SphereCollider>().enabled = true;
        m_bombSc.angle = angle;
        m_bombSc.ThrowingBall(angle, playerPos);
        isThrowing = true;
        isHold = false;
        isTouchBomb = false;
        m_bombSc = null;
        m_bomb = null;
    }

    IEnumerator ExitCollisionSwitch_Corutine()
    {
        exitCollision.enabled = false;
        yield return new WaitForSeconds(0.7f);
        exitCollision.enabled = true;
    }

    private void UpdateMovement()
    {
        if (!canMove)
        { //Return if player can't move
            return;
        }
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

            DropBomb(pos, bombId++, bombType, firePower, isKick);
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


    /// <summary>
    /// Drops a bomb beneath the player
    /// </summary>
    protected void DropBomb(Vector3 bombPos, int bombId, int bombType, int firePower, bool isKick)
    {
        BombManager.Instance.BombInstantiate(bombPos, bombId, 1, bombType, firePower);//, isKick);
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
            isTouchBomb = true;
        //    m_bomb = other.gameObject;
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
            isTouchBomb = false;
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
            StartCoroutine(Die());
        }
    }

    IEnumerator FreezeMove()
    {
        localClock.localTimeScale = 0;
        yield return new WaitForSeconds(2);
        localClock.localTimeScale = 1;
    }

    public virtual IEnumerator Die()
    {
        boxCollider_Collision.isTrigger = false;
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
        */
     //   Debug.DrawRay(transform.position, transform.forward * .7f, Color.blue, .1f);
        
    }
}
