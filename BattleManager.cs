using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BattleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txt_timeText;
    [SerializeField] Text txt_player1Score;
    [SerializeField] Text txt_player2Score;
    [SerializeField] Text txt_player3Score;
    [SerializeField] Text txt_player4Score;
    private double m_double_time;
    private float m_float_time;
    public int int_player1Score;
    public int int_player2Score;
    public int int_player3Score;
    public int int_player4Score;

    // Start is called before the first frame update
    void Start()
    {
        GManager.Instance.PlayerInstantiate();
        Debug.Log("バトルモード" + PhotonNetwork.CurrentRoom.GetBattleMode());
        if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.TimeMode && !GManager.Instance.isOffLine)
        {
            m_double_time = PhotonNetwork.Time;
            m_float_time = PhotonNetwork.CurrentRoom.GetTimeUpdate();
            txt_timeText.text = PhotonNetwork.CurrentRoom.GetTimeUpdate().ToString();
            txt_player1Score.text = 0.ToString();
            txt_player2Score.text = 0.ToString();
            txt_player3Score.text = 0.ToString();
            txt_player4Score.text = 0.ToString();
        }
        else if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.SurvivalMode && !GManager.Instance.isOffLine)
        {
            txt_timeText.text = "∞";
            txt_player1Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
            txt_player2Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
            txt_player3Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
            txt_player4Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && m_double_time + 5 < PhotonNetwork.Time&&PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.TimeMode && !GManager.Instance.isOffLine)
        {
            m_double_time = PhotonNetwork.Time;
            m_float_time -= 5;
            PhotonNetwork.CurrentRoom.SetTimeUpdate(m_float_time);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.TimeMode && !GManager.Instance.isOffLine)
        {
            txt_timeText.text = PhotonNetwork.CurrentRoom.GetTimeUpdate().ToString();
        }
    }

    public void UpdateScore(int score, int Player)
    {
        if (Player == 1)
        {
            int_player1Score += score;
            txt_player1Score.text = int_player1Score.ToString();
        }
        else if (Player == 2)
        {
            int_player2Score += score;
            txt_player2Score.text = int_player2Score.ToString();
        }
        else if (Player == 3)
        {
            int_player3Score += score;
            txt_player3Score.text = int_player3Score.ToString();
        }
        else if (Player == 4)
        {
            int_player4Score += score;
            txt_player4Score.text = int_player4Score.ToString();
        }
        else
        {
            return;
        }
    }

}
