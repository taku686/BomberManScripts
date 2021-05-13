using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BattleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text m_txt_timeText;
    [SerializeField] Text m_txt_player1Score;
    [SerializeField] Text m_txt_player2Score;
    [SerializeField] Text m_txt_player3Score;
    [SerializeField] Text m_txt_player4Score;
    [SerializeField] Text m_txt_resultText;
    private double m_double_time;
    private float m_float_time;
 //   public int int_player1Score;
 //   public int int_player2Score;
 //   public int int_player3Score;
 //   public int int_player4Score;
    public int[] m_array_playerScore= new int[4];

    // Start is called before the first frame update
    void Start()
    {
        GManager.Instance.PlayerInstantiate();
        Debug.Log("バトルモード" + PhotonNetwork.CurrentRoom.GetBattleMode());
        if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.TimeMode && !GManager.Instance.isOffLine)
        {
            m_double_time = PhotonNetwork.Time;
            m_float_time = PhotonNetwork.CurrentRoom.GetTimeUpdate();
            m_txt_timeText.text = PhotonNetwork.CurrentRoom.GetTimeUpdate().ToString();
            m_txt_player1Score.text = 0.ToString();
            m_txt_player2Score.text = 0.ToString();
            m_txt_player3Score.text = 0.ToString();
            m_txt_player4Score.text = 0.ToString();
        }
        else if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.SurvivalMode && !GManager.Instance.isOffLine)
        {
            m_txt_timeText.text = "∞";
            m_txt_player1Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
            m_txt_player2Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
            m_txt_player3Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
            m_txt_player4Score.text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
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
            m_txt_timeText.text = PhotonNetwork.CurrentRoom.GetTimeUpdate().ToString();
        }
        if (PhotonNetwork.CurrentRoom.GetTimeUpdate() <= 0)
        {
            int highScore = 0;
            for (int i = 0; i < 4; i++)
            {
                if (highScore < m_array_playerScore[i])
                {
                    m_txt_resultText.enabled = true;
                    highScore = m_array_playerScore[i];
                    m_txt_resultText.text = $"Player{i + 1}勝利";
                }
            }

        }
    }

    public void UpdateScore(int score, int Player)
    {
        if (Player == 1)
        {
            m_array_playerScore[0] += score;
            m_txt_player1Score.text = m_array_playerScore[0].ToString();
        }
        else if (Player == 2)
        {
            m_array_playerScore[1] += score;
            m_txt_player2Score.text = m_array_playerScore[1].ToString();
        }
        else if (Player == 3)
        {
            m_array_playerScore[2] += score;
            m_txt_player3Score.text = m_array_playerScore[2].ToString();
        }
        else if (Player == 4)
        {
            m_array_playerScore[3] += score;
            m_txt_player4Score.text = m_array_playerScore[3].ToString();
        }
        else
        {
            return;
        }
    }

}
