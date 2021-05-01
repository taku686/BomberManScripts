using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class AreaClockMove : MonoBehaviourPunCallbacks
{
    Vector3 center = new Vector3(4, 0, 5);
    float x_range = 7;
    float z_ange = 6;
    float time;
    float x_value;
    float z_value;
  
    // Update is called once per frame
    void Update()
    {
        if (Time.time > time +3)
        {
            time = Time.time;
            x_value = center.x + Random.Range(-x_range, x_range);
            z_value = center.z + Random.Range(-z_ange, z_ange);
            photonView.RPC(nameof(Move), RpcTarget.All, x_value, z_value);
        }
    }

    [PunRPC]
    private void Move(float x,float z)
    {
        transform.DOMove(new Vector3(x, center.y, z), 3);
    }
}
