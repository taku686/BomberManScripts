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
        if (Bomb.isCounter&&photonView.IsMine)
        {
      //      Debug.Log("反射");
            Bomb.isCounter = false;
            StartCoroutine(CreateExplosions(transform.forward));
        }
    }

    protected virtual IEnumerator CreateExplosions(Vector3 direction)
    {
        // 2 マス分ループする
        for (int i = 1; i < 8; i++)
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
            if (!hit.collider)
            {
                // 爆風を広げるために、
                // 爆発エフェクトのオブジェクトを作成
                PhotonNetwork.Instantiate
                  (
                      explosionPrefab.name,
                      transform.position + (i * direction),
                      explosionPrefab.transform.rotation
                  );
            }
            // 爆風を広げた先に壊れる壁が存在する場合
            else if (hit.collider.CompareTag("BreakingWall"))
            {
                PhotonNetwork.Instantiate
                 (
                     explosionPrefab.name,
                     transform.position + (i * direction),
                     explosionPrefab.transform.rotation
                 );
                break;
            }
            // 爆風を広げた先に壁が存在する場合
            else if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("CounterEffect"))
            {
                // 爆風はこれ以上広げない
                //      Debug.Log("爆風はこれ以上広げない");
                break;
            }


            // 0.05 秒待ってから、次のマスに爆風を広げる
            yield return new WaitForSeconds(0.05f);
        }
    }
}
