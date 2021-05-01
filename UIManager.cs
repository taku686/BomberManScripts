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
    public GameObject characterSelectPanel;
    public GameObject masterCharacterSelectPanel;
    public GameObject stageSelectPanel;
    public GameObject playerNumSelectPanel;
    public GameObject characterClone;
    public static byte playerNum=2;
    private RoomOptions roomOptions = new RoomOptions();
    
    private int characterNum = 1;
    private string m_roomName;
    private string newRoomName;

    private void Start()
    {
        for (int i = 2; i < 5; i++)
        {
            GameObject gridClone = Instantiate(playerNumGrid, playerNumContent.transform);
            gridClone.GetComponentInChildren<Text>().text = i.ToString();
        }
        for (int j = 1; j < 5; j++)
        {
            GameObject gridClone = Instantiate(stageSelectGrid, stageSelectContent.transform);
            gridClone.name = j.ToString();
        }
        for (int k = 1; k < 10; k++)
        {
            GameObject gridClone = Instantiate(playerSelectButton, playerSelectContent.transform);
            gridClone.GetComponentInChildren<Text>().text = k.ToString();
        }
    }

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

    public void OnStageSelectRightArrow_Click_()
    {     
        GManager.Instance.stageNum++;
        Debug.Log(GManager.Instance.stageNum);
        if (GManager.Instance.stageNum > 3)
        {
            GManager.Instance.stageNum = 1;
        }
        stageImage.sprite = stageSouceImages[GManager.Instance.stageNum-1];
    }

    public void OnStageSelectLeftArrow_Click_()
    {
        GManager.Instance.stageNum--;
        if (GManager.Instance.stageNum < 1)
        {
            GManager.Instance.stageNum = 3;
        }
        stageImage.sprite = stageSouceImages[GManager.Instance.stageNum-1];
    }



    public void On_Click_PlayerNum()
    {
        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = playerNum;
        newRoomName = "Room" + Random.Range(0, 10000);
        characterClone = Instantiate(GManager.Instance.CharacterInstantiate(1), new Vector3(-13, 0.5f, -8.5f), Quaternion.Euler(0, 180, 0));
        playerNumSelectPanel.SetActive(false);
        masterCharacterSelectPanel.SetActive(true);
    }

    public void On_Click_Master_Character_Decide()
    {
        GManager.Instance.selectedCharacterNum = characterNum;
        Destroy(characterClone);
        masterCharacterSelectPanel.SetActive(false);
        stageSelectPanel.SetActive(true);
    }

    public void OnClick_CreateRoom()
    {
        stageSelectPanel.SetActive(false);
        PhotonNetwork.CreateRoom(newRoomName, roomOptions);
    }

    public void On_Click_Character_Decide()
    {
        GManager.Instance.selectedCharacterNum = characterNum;
        PhotonNetwork.JoinRoom(m_roomName);
        characterSelectPanel.SetActive(false);
    }

}
