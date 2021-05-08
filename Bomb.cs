using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkeletonEditor;
using DG.Tweening;
using Photon.Pun;
using System.Linq;

public class Bomb : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected int ownerId;
    public GameObject explosionPrefab;
    public GameObject explosionPrefab_sub;
    public LayerMask levelMask;
    protected bool exploded;
    protected ItemManager itemManager;
    protected float bombTimer;
    protected GameObject player;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected float throwingSpeed;
    protected Vector3 targetPosition;
    public bool isBombWait;
    public float angle;
    [SerializeField] public SphereCollider sphereCollider;
    protected bool isChain;
    public static bool isCounter;
    public bool isSkipOver;
    public bool isForward;
    public bool isBack;
    public bool isRight;
    public bool isLeft;
 public int m_firePower;
    public bool m_isKick;
    private int bombType;
    //   public bool isHold;
    public int Id { get { return id; } private set { id = value; } }
    public int OwnerId { get { return ownerId; } private set { ownerId = value; } }

    public int BombType { get { return bombType; } private set { bombType = value; } }

    public bool Equals(int id, int ownerId) => id == Id && ownerId == OwnerId;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        StartCoroutine(Explode());
    }


    public virtual void Initialized(int id, int ownerId,int firePower,int bombType,bool isKick)
    {
        Id = id;
        OwnerId = ownerId;
        m_firePower = firePower;
        m_isKick = isKick;
        BombType = bombType;
    }

    public void ThrowingBall(float angle,Vector3 playerPos)
    {
        //  StartCoroutine(ThrowingBall_Corutine(angle,playerPos));
        sphereCollider.isTrigger = true;
        if (angle == 0)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(playerPos.x), 0.5f, Mathf.RoundToInt(playerPos.z + 3));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }
        else if (angle == 90)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(playerPos.x + 3), 0.5f, Mathf.RoundToInt(playerPos.z));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }
        else if (angle == 180)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(playerPos.x), 0.5f, Mathf.RoundToInt(playerPos.z - 3));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }
        else if (angle == 270)
        {
            targetPosition = new Vector3(Mathf.RoundToInt(playerPos.x - 3), 0.5f, Mathf.RoundToInt(playerPos.z));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }
    }
 
    protected virtual IEnumerator Explode()
    {
        if (!isChain)
        {
            while (bombTimer < 3)
            {
                yield return new WaitForSeconds(0.5f);
                if (!isBombWait)
                {
                    //    Debug.Log(bombTimer);
                    bombTimer += 0.5f;
                }
            }
        }

        // 爆弾の位置に爆発エフェクトを作成
       Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 爆弾を非表示にする
        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        StartCoroutine(CreateExplosions(Vector3.forward)); // 上に広げる
        StartCoroutine(CreateExplosions(Vector3.right)); // 右に広げる
        StartCoroutine(CreateExplosions(Vector3.back)); // 下に広げる
        StartCoroutine(CreateExplosions(Vector3.left)); // 左に広げる
       
        BombManager.Instance.BombRemove(Id, OwnerId);                                          
        Destroy(gameObject, .2f);
    }

    protected void OnDestroy()
    {
        if (OwnerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            BombManager.Instance.BombConsumptionCount(-1);
        }
    }

    // 爆風を広げる
    protected virtual IEnumerator CreateExplosions(Vector3 direction)
    {
        // 2 マス分ループする
        for (int i = 1; i < m_firePower; i++)
        {
            // ブロックとの当たり判定の結果を格納する変数
            RaycastHit hit;

            // 爆風を広げた先に何か存在するか確認
            Physics.Raycast
           (
               transform.position,
               direction,
                out hit,
               i,
               levelMask
           );

            // 爆風を広げた先に何も存在しない場合
            if (!hit.collider || hit.collider.CompareTag("Player"))
            {
                // 爆風を広げるために、
                // 爆発エフェクトのオブジェクトを作成
                Instantiate
                   (
                       explosionPrefab,
                       transform.position + (i * direction),
                       explosionPrefab.transform.rotation
                   );
            }
            // 爆風を広げた先に壊れる壁が存在する場合
            else if (hit.collider.CompareTag("BreakingWall"))
            {
                Instantiate
                 (
                     explosionPrefab,
                     transform.position + (i * direction),
                     explosionPrefab.transform.rotation
                 );
                break;
            }
            // 爆風を広げた先に壁が存在する場合
            else if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("ShieldEffect"))
            {
                // 爆風はこれ以上広げない
                //  Debug.Log("爆風はこれ以上広げない");
                break;
            }
            else if (hit.collider.CompareTag("CounterEffect"))
            {
                isCounter = true;
                // 爆風はこれ以上広げない
                //      Debug.Log("爆風はこれ以上広げない");
                break;
            }
            else
            {
                Instantiate
                 (
                     explosionPrefab,
                     transform.position + (i * direction),
                     explosionPrefab.transform.rotation
                 );
            }
            // 0.05 秒待ってから、次のマスに爆風を広げる
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 他のオブジェクトがこの爆弾に当たったら呼び出される
    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))//&&!isHold)
        {
            StopCoroutine(Explode());
            isChain = true;
            StartCoroutine(Explode());
        }
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
        if ((other.CompareTag("BreakingWall") || other.CompareTag("Wall")) && (PlayerBase.isThrowing || PlayerBase_OffLine.isThrowing || isSkipOver))
        {
            //    Debug.Log("壁接触");
            if (angle == 0 || isForward)
            {
                targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z + 1));
                transform.DOJump(targetPosition, 2, 1, throwingSpeed);
            }
            else if (angle == 90 || isRight)
            {
                targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x + 1), 0.5f, Mathf.RoundToInt(transform.position.z));
                transform.DOJump(targetPosition, 2, 1, throwingSpeed);
            }
            else if (angle == 180 || isBack)
            {
                targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z - 1));
                transform.DOJump(targetPosition, 2, 1, throwingSpeed);
            }
            else if (angle == 270 || isLeft)
            {
                targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x - 1), 0.5f, Mathf.RoundToInt(transform.position.z));
                transform.DOJump(targetPosition, 2, 1, throwingSpeed);
            }
        }
        else if (other.CompareTag("OutWall") && (PlayerBase.isThrowing || PlayerBase_OffLine.isThrowing || isSkipOver))
        {
            StartCoroutine(BombTeleportaion());
        }
        if (other.CompareTag("Ground") && (PlayerBase.isThrowing || PlayerBase_OffLine.isThrowing || isSkipOver))
        {
            PlayerBase.isThrowing = false;
            PlayerBase_OffLine.isThrowing = false;
            isBombWait = false;
            sphereCollider.isTrigger = false;
            isSkipOver = false;
            isForward = false;
            isBack = false;
            isRight = false;
            isLeft = false;
        }
        if (other.CompareTag("VanishSkill"))
        {
            StopCoroutine(Explode());
            BombManager.Instance.BombRemove(Id, OwnerId);
            Instantiate(explosionPrefab_sub, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.CompareTag("DetonationEffect"))
        {
            StopCoroutine(Explode());
            isChain = true;
            StartCoroutine(Explode());
        }
        if (other.CompareTag("SkipOverEffect"))
        {
            //    Debug.Log("ふっとばし");
            angle = 1;
            isSkipOver = true;
            sphereCollider.isTrigger = true;
            isBombWait = true;
            SkipOver(other.transform);
        }
    }

    protected void SkipOver(Transform target)
    {
        if (target.position.x == Mathf.RoundToInt(transform.position.x) && target.position.z < transform.position.z)
        {
            isForward = true;
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z + 5));
            transform.DOJump(targetPosition, 2, 1, throwingSpeed);
        }
        else if (target.position.x == Mathf.RoundToInt(transform.position.x) && target.position.z > transform.position.z)
        {
            isBack = true;
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z - 5));
            transform.DOJump(targetPosition, 2, 1, throwingSpeed);
        }
        else if (target.position.x < transform.position.x && target.position.z == Mathf.RoundToInt(transform.position.z))
        {
            isRight = true;
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x + 5), 0.5f, Mathf.RoundToInt(transform.position.z));
            transform.DOJump(targetPosition, 2, 1, throwingSpeed);
        }
        else if (target.position.x > transform.position.x && target.position.z == Mathf.RoundToInt(transform.position.z))
        {
            isLeft = true;
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x - 5), 0.5f, Mathf.RoundToInt(transform.position.z));
            transform.DOJump(targetPosition, 2, 1, throwingSpeed);
        }
    }

    IEnumerator BombTeleportaion()
    {
        if (angle == 0 || isForward)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.DOKill();
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            yield return new WaitForSeconds(0.4f);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, -1);
            //    Debug.Log("四捨五入" + Mathf.RoundToInt(transform.position.z + 1));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }
        else if (angle == 90 || isRight)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.DOKill();
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(-2, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(0.4f);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            targetPosition = new Vector3(-1, 0.5f, Mathf.RoundToInt(transform.position.z));
            //    Debug.Log("四捨五入" + Mathf.RoundToInt(transform.position.z + 1));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }
        else if (angle == 180 || isBack)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.DOKill();
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(transform.position.x, transform.position.y, 10);
            yield return new WaitForSeconds(0.4f);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            targetPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, 9);
            //    Debug.Log("四捨五入" + Mathf.RoundToInt(transform.position.z + 1));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }
        else if (angle == 270 || isLeft)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.DOKill();
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(0.4f);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            targetPosition = new Vector3(10, 0.5f, Mathf.RoundToInt(transform.position.z));
            //    Debug.Log("四捨五入" + Mathf.RoundToInt(transform.position.z + 1));
            transform.DOJump(targetPosition, 1, 1, throwingSpeed);
        }

    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerBase.isHold)
        {
            player = null;
        }
    }
}
