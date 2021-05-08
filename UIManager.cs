using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviour
{
    [SerializeField] PhotonManager photonManager;
    [SerializeField] GameObject playerNumGrid;
    [SerializeField] GameObject stageSelectGrid;
    [SerializeField] GameObject playerNumContent;
    [SerializeField] GameObject stageSelectContent;
    [SerializeField] GameObject playerSelectButton;
    [SerializeField] GameObject playerSelectContent;
    [SerializeField] List<Sprite> stageSouceImages;
    [SerializeField] Image stageImage;
    [SerializeField] GameObject obj_BMName_Point;
    [SerializeField] GameObject obj_BMExplanation_Point;
    [SerializeField] GameObject obj_BMName_Survival;
    [SerializeField] GameObject obj_BMExplanation_Survival;
    [SerializeField] GameObject obj_HeartText;
    [SerializeField] GameObject obj_TimeText;
    [SerializeField] GameObject obj_HeartNum;
    [SerializeField] GameObject obj_TimeNum;
    [SerializeField] Text txt_TimeNum;
    [SerializeField] Text txt_HeartNum;
    [SerializeField] Text txt_ParticipationNum;
    [SerializeField] Text txt_RoundNum;
    public GameObject characterSelectPanel;
    public GameObject masterCharacterSelectPanel;
    public GameObject stageSelectPanel;
    public GameObject playerNumSelectPanel;
    public GameObject characterClone;
    private RoomOptions roomOptions = new RoomOptions();

    private int characterNum = 1;
    private string m_roomName;
    private string newRoomName;
    private int int_BattleModeNum=10000;
    private int int_HeartOrTimeNum = 2;
    private byte byte_ParticipationNum = 2;
    private int int_RoundNum = 1;

  
    //----------------ルール設定----------------

    public void On_Click_RightArrow_BattleMode()
    {
        int_BattleModeNum++;
        if (int_BattleModeNum % 2 == 1)
        {
            GManager.Instance.battleMode = GManager.BattleMode.TimeMode;
            obj_BMExplanation_Point.SetActive(true);
            obj_BMName_Point.SetActive(true);
            obj_BMExplanation_Survival.SetActive(false);
            obj_BMName_Survival.SetActive(false);
            obj_HeartText.SetActive(false);
            obj_TimeText.SetActive(true);
            obj_HeartNum.SetActive(false);
            obj_TimeNum.SetActive(true);
        }
        else if (int_BattleModeNum % 2 == 0)
        {
            GManager.Instance.battleMode = GManager.BattleMode.SurvivalMode;
            obj_BMExplanation_Point.SetActive(false);
            obj_BMName_Point.SetActive(false);
            obj_BMExplanation_Survival.SetActive(true);
            obj_BMName_Survival.SetActive(true);
            obj_HeartText.SetActive(true);
            obj_TimeText.SetActive(false);
            obj_HeartNum.SetActive(true);
            obj_TimeNum.SetActive(false);
        }
    }

    public void On_Click_LeftArrow_BattleMode()
    {
        int_BattleModeNum--;
        if (int_BattleModeNum % 2 == 1)
        {
            GManager.Instance.battleMode = GManager.BattleMode.TimeMode;
            obj_BMExplanation_Point.SetActive(true);
            obj_BMName_Point.SetActive(true);
            obj_BMExplanation_Survival.SetActive(false);
            obj_BMName_Survival.SetActive(false);
            obj_HeartText.SetActive(false);
            obj_TimeText.SetActive(true);
            obj_HeartNum.SetActive(false);
            obj_TimeNum.SetActive(true);
        }
        else if (int_BattleModeNum % 2 == 0)
        {
            GManager.Instance.battleMode = GManager.BattleMode.SurvivalMode;
            obj_BMExplanation_Point.SetActive(false);
            obj_BMName_Point.SetActive(false);
            obj_BMExplanation_Survival.SetActive(true);
            obj_BMName_Survival.SetActive(true);
            obj_HeartText.SetActive(true);
            obj_TimeText.SetActive(false);
            obj_HeartNum.SetActive(true);
            obj_TimeNum.SetActive(false);
        }
    }

    public void On_Click_RightArrow_TimeOrHeart()
    {
        int_HeartOrTimeNum++;
        if (int_HeartOrTimeNum > 5)
        {
            int_HeartOrTimeNum = 2;
        }
        txt_HeartNum.text = $"{int_HeartOrTimeNum}機";
        txt_TimeNum.text = $"{int_HeartOrTimeNum}分";       
    }

    public void On_Click_LeftArrow_TimeOrHeart()
    {
        int_HeartOrTimeNum--;
        if (int_HeartOrTimeNum < 2)
        {
            int_HeartOrTimeNum = 5;
        }
        txt_HeartNum.text = $"{int_HeartOrTimeNum}機";
        txt_TimeNum.text = $"{int_HeartOrTimeNum}分";
    }

    public void On_Click_RightArrow_Participation()
    {
        byte_ParticipationNum++;
        if (byte_ParticipationNum > 4)
        {
            byte_ParticipationNum = 2;
        }
        txt_ParticipationNum.text = $"{byte_ParticipationNum}人";
    }

    public void On_Click_LeftArrow_Participation()
    {
        byte_ParticipationNum--;
        if (byte_ParticipationNum < 2)
        {
            byte_ParticipationNum = 4;
        }
        txt_ParticipationNum.text = $"{byte_ParticipationNum}人";
    }

    public void On_Click_RightArrow_Round()
    {
        int_RoundNum++;
        if (int_RoundNum > 5)
        {
            int_RoundNum = 1;
        }
        txt_RoundNum.text = $"{int_RoundNum}ラウンド";
    }

    public void On_Click_LeftArrow_Round()
    {
        int_RoundNum--;
        if (int_RoundNum < 1)
        {
            int_RoundNum = 5;
        }
        txt_RoundNum.text = $"{int_RoundNum}ラウンド";
    }

    public void On_Click_PlayerNum()
    {
        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte_ParticipationNum;
        newRoomName = "Room" + Random.Range(0, 10000);
        GManager.Instance.heart = int_HeartOrTimeNum;
        GManager.Instance.time = int_HeartOrTimeNum;
        GManager.Instance.round = int_RoundNum;
        characterClone = Instantiate(GManager.Instance.CharacterInstantiate(1), new Vector3(-13, 0.5f, -8.5f), Quaternion.Euler(0, 180, 0));
        playerNumSelectPanel.SetActive(false);
        masterCharacterSelectPanel.SetActive(true);
    }

    //----------------プレイヤー設定----------------

    public void OnPlayerSelectPanel_Click(string roomName)
    {
        characterSelectPanel.SetActive(true);
        characterClone = Instantiate(GManager.Instance.CharacterInstantiate(1), new Vector3(-13, 0.5f, -8.5f), Quaternion.Euler(0, 180, 0));
        m_roomName = roomName;
    }

    public void OnRightArrow_Click()
    {
        Destroy(characterClone);
        characterNum++;
        if (characterNum > 9)
        {
            characterNum = 1;
        }
        characterClone = Instantiate(GManager.Instance.CharacterInstantiate(characterNum), new Vector3(-13, 0.5f, -8.5f), Quaternion.Euler(0, 180, 0));
    }

    public void OnLeftArrow_Click()
    {
        Destroy(characterClone);
        characterNum--;
        if (characterNum < 1)
        {
            characterNum = 9;
        }
        characterClone = Instantiate(GManager.Instance.CharacterInstantiate(characterNum), new Vector3(-13, 0.5f, -8.5f), Quaternion.Euler(0, 180, 0));
    }
    public void On_Click_Master_Character_Decide()
    {
        GManager.Instance.selectedCharacterNum = characterNum;
        Destroy(characterClone);
        masterCharacterSelectPanel.SetActive(false);
        stageSelectPanel.SetActive(true);
    }

    public void On_Click_Character_Decide()
    {
        GManager.Instance.selectedCharacterNum = characterNum;
        PhotonNetwork.JoinRoom(m_roomName);
        characterSelectPanel.SetActive(false);
    }

    //----------------ステージ設定----------------
    public void OnStageSelectRightArrow_Click_()
    {
        GManager.Instance.stageNum++;
        //      Debug.Log(GManager.Instance.stageNum);
        if (GManager.Instance.stageNum > 3)
        {
            GManager.Instance.stageNum = 1;
        }
        stageImage.sprite = stageSouceImages[GManager.Instance.stageNum - 1];
    }

    public void OnStageSelectLeftArrow_Click_()
    {
        GManager.Instance.stageNum--;
        if (GManager.Instance.stageNum < 1)
        {
            GManager.Instance.stageNum = 3;
        }
        stageImage.sprite = stageSouceImages[GManager.Instance.stageNum - 1];
    }

    public void OnClick_CreateRoom()
    {
        stageSelectPanel.SetActive(false);
        PhotonNetwork.CreateRoom(newRoomName, roomOptions);
    }
}
