using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class DiffuseBomb : Bomb
{
    protected override IEnumerator Explode()
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
        StartCoroutine(CreateExplosions(Vector3.forward));
        BombManager.Instance.BombRemove(Id, OwnerId);
        Destroy(gameObject, .2f);
    }

    protected override IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 0; i < m_firePower; i++)
        {
            for (int j = 0; j < m_firePower; j++)
            {
                Instantiate
               (
                   explosionPrefab,
                   new Vector3(transform.position.x + i - m_firePower / 2, transform.position.y, transform.position.z + j - m_firePower / 2),
                   explosionPrefab.transform.rotation
               );
            }
        }
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(m_firePower / 2, m_firePower / 2, m_firePower / 2), Vector3.up, Quaternion.identity, 1, levelMask);
        foreach (var hit in hits)
        {
            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("CounterEffect"))
                {
                    isCounter = true;
                    //      Debug.Log("カウンター発動");
                }
            }
        }
        yield return new WaitForSeconds(0.05f);
    }
}
