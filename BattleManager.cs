using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BattleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text m_txt_timeText;
    [SerializeField] GameObject[] m_obj_PlayerScore = new GameObject[4];
    [SerializeField] Text[] m_txt_playerScore = new Text[4];
    [SerializeField] Text m_txt_resultText;
    private double m_double_time;
    private float m_float_time;
 
    public int[] m_array_playerScore= new int[4];

    // Start is called before the first frame update
    void Start()
    {
        GManager.Instance.PlayerInstantiate();
        //      Debug.Log("バトルモード" + PhotonNetwork.CurrentRoom.GetBattleMode());
        if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.TimeMode && !GManager.Instance.isOffLine)
        {
            m_float_time = PhotonNetwork.CurrentRoom.GetTimeUpdate();
            m_txt_timeText.text = PhotonNetwork.CurrentRoom.GetTimeUpdate().ToString();
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                m_obj_PlayerScore[i].SetActive(true);
                m_txt_playerScore[i].text = 0.ToString();
            }
        }
        else if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.SurvivalMode && !GManager.Instance.isOffLine)
        {
            m_txt_timeText.text = "∞";
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                m_obj_PlayerScore[i].SetActive(true);
                m_txt_playerScore[i].text = PhotonNetwork.CurrentRoom.GetHeartNum().ToString();
                m_array_playerScore[i] = PhotonNetwork.CurrentRoom.GetHeartNum();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.InRoom) { return; }
        // まだゲームの開始時刻が設定されていない場合は更新しない
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

        float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);
        m_txt_timeText.text = (m_float_time - elapsedTime).ToString("f0");
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.CurrentRoom.GetTimeUpdate() <= 0 && PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.TimeMode)
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
            m_txt_playerScore[0].text = m_array_playerScore[0].ToString();
        }
        else if (Player == 2)
        {
            m_array_playerScore[1] += score;
            m_txt_playerScore[1].text = m_array_playerScore[1].ToString();
        }
        else if (Player == 3)
        {
            m_array_playerScore[2] += score;
            m_txt_playerScore[2].text = m_array_playerScore[2].ToString();
        }
        else if (Player == 4)
        {
            m_array_playerScore[3] += score;
            m_txt_playerScore[3].text = m_array_playerScore[3].ToString();
        }
        else
        {
            return;
        }
        if (PhotonNetwork.CurrentRoom.GetBattleMode() == (int)GManager.BattleMode.SurvivalMode)
        {
            int deadPlayer = 0;
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                if (m_array_playerScore[i] <= 0)
                {
                    deadPlayer++;
                }
            }
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                if (deadPlayer == PhotonNetwork.CurrentRoom.MaxPlayers - 1 && m_array_playerScore[i] > 0)
                {
                    m_txt_resultText.enabled = true;
                    m_txt_resultText.text = $"Player{i + 1}勝利";
                }
            }
        }
    }
}
