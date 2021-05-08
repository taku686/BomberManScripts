using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BattleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txt_imeText;
    private double m_double_time;
    private float m_float_time;
    // Start is called before the first frame update
    void Start()
    {
        GManager.Instance.PlayerInstantiate();
        if (GManager.Instance.battleMode == GManager.BattleMode.TimeMode&&!GManager.Instance.isOffLine)
        {
            m_double_time = PhotonNetwork.Time;
            m_float_time = PhotonNetwork.CurrentRoom.GetTimeUpdate();
            txt_imeText.text = PhotonNetwork.CurrentRoom.GetTimeUpdate().ToString();
        }
        else if(GManager.Instance.battleMode == GManager.BattleMode.SurvivalMode && !GManager.Instance.isOffLine)
        {
            txt_imeText.text = "∞";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && m_double_time + 5 < PhotonNetwork.Time&&GManager.Instance.battleMode == GManager.BattleMode.TimeMode && !GManager.Instance.isOffLine)
        {
            m_double_time = PhotonNetwork.Time;
            m_float_time -= 5;
            PhotonNetwork.CurrentRoom.SetTimeUpdate(m_float_time);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (GManager.Instance.battleMode == GManager.BattleMode.TimeMode && !GManager.Instance.isOffLine)
        {
            txt_imeText.text = PhotonNetwork.CurrentRoom.GetTimeUpdate().ToString();
        }
    }

}
