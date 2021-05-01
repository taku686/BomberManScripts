using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject stage3;
    [SerializeField] GameObject areaClock;
    [SerializeField] int stageNum;//後でPhotonManager.stageNumberでステージが変更できるように変える
    // Start is called before the first frame update
    void Start()
    {
        stageNum = PhotonNetwork.CurrentRoom.GetStageNum();
        if (stageNum == 1)
        {
            Instantiate(stage1, Vector3.zero, stage1.transform.rotation);
        }
        else if(stageNum == 2)
        {
            Instantiate(stage2, Vector3.zero, stage2.transform.rotation);
        }
        else if (stageNum == 3)
        {
            Instantiate(stage3, Vector3.zero, stage3.transform.rotation);
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject areaClockClone = PhotonNetwork.Instantiate(areaClock.name, new Vector3(4, 0, 5), areaClock.transform.rotation);
                areaClockClone.GetComponent<AreaClockMove>().enabled = true;
            }
        }
    }
}
