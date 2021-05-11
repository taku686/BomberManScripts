using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PenetrationBomb : Bomb
{

    protected override IEnumerator CreateExplosions(Vector3 direction)
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


            if (!hit.collider || hit.collider.CompareTag("BreakingWall") || hit.collider.CompareTag("Player"))
            {
                // 爆風を広げるために、
                // 爆発エフェクトのオブジェクトを作成
                /*
                Instantiate
                 (
                     explosionPrefab,
                     transform.position + (i * direction),
                     explosionPrefab.transform.rotation
                 );
                */
                BombManager.Instance.InstantiateExplosionEffect(transform.position + (i * direction), m_explosionNum);
            }
            else if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("ShieldEffect"))
            {
                // 爆風はこれ以上広げない
                Debug.Log("爆風はこれ以上広げない");
                break;
            }
            else if (hit.collider.CompareTag("CounterEffect"))
            {
                isCounter = true;
                // 爆風はこれ以上広げない
                //      Debug.Log("爆風はこれ以上広げない");
                break;
            }

            // 0.05 秒待ってから、次のマスに爆風を広げる
            yield return new WaitForSeconds(0.05f);
        }
    }
}
