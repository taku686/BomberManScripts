using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Counter : MonoBehaviourPunCallbacks
{
   
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] LayerMask levelMask;
    // Start is called before the first frame updat

    private void Update()
    {
        if (Bomb.isCounter)
        {
   //         Debug.Log("反射");
            Bomb.isCounter = false;
            StartCoroutine(CreateExplosions(-transform.right, transform.position));
        }
    }

    private void OnDrawGizmos()
    {
  //      Debug.DrawRay(transform.position, -transform.right, Color.blue, .1f);
    }

    protected IEnumerator CreateExplosions(Vector3 direction, Vector3 pos)
    {
        // 2 マス分ループする
        for (int i = 1; i < 8; i++)
        {
            // ブロックとの当たり判定の結果を格納する変数
            RaycastHit hit;

            // 爆風を広げた先に何か存在するか確認
            Physics.Raycast
           (
              pos,
               direction,
                out hit,
               i,
               levelMask
           );

            // 爆風を広げた先に何も存在しない場合
            if (!hit.collider || hit.collider.CompareTag("Player"))
            {
                if (!GManager.Instance.isOffLine)
                {
                    PhotonNetwork.Instantiate
                       (
                           explosionPrefab.name,
                          pos + (i * direction),
                           explosionPrefab.transform.rotation
                       );
                }
                else
                {
                    Instantiate
                       (
                           explosionPrefab,
                          pos + (i * direction),
                           explosionPrefab.transform.rotation
                       );
                }
            }

            else if (hit.collider.CompareTag("BreakingWall"))
            {
                if (!GManager.Instance.isOffLine)
                {
                    PhotonNetwork.Instantiate
                  (
                      explosionPrefab.name,
                     pos + (i * direction),
                      explosionPrefab.transform.rotation
                  );
                }
                else
                {
                    Instantiate
                   (
                    explosionPrefab,
                    pos + (i * direction),
                    explosionPrefab.transform.rotation
                   );

                }
                break;
            }

            else if (hit.collider.CompareTag("Wall"))
            {

                break;
            }
            else
            {
                if (!GManager.Instance.isOffLine)
                {
                    PhotonNetwork.Instantiate
                   (
                     explosionPrefab.name,
                     transform.position + (i * direction),
                     explosionPrefab.transform.rotation
                   );
                }
                else
                {
                    Instantiate
                     (
                       explosionPrefab,
                       pos + (i * direction),
                       explosionPrefab.transform.rotation
                     );
                }
            }
            // 0.05 秒待ってから、次のマスに爆風を広げる
            yield return new WaitForSeconds(0.05f);
        }
    }
}
