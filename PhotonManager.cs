using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using System.Linq;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject initPanel;
    [SerializeField] GameObject selectPanel;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject waitingPanel;
    [SerializeField] Text timer;
    [SerializeField] Button leftButton;
    public List<GameObject> playerObj;
  //  public static byte playerNumber=2;
    public static int stageNumber=1;
    public  int itemNumber;
    public  bool isCreateRoom;
    public  bool isEnterRoom;
    private float time;
    private int timeNum = 30;
    private bool isSceneMove;
    private GameObject playerClone;
    private void Awake()
    {
        isSceneMove = false;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
  
    private void Update()
    {
        if (waitingPanel.activeSelf==true && PhotonNetwork.CurrentRoom.PlayerCount>1)
        {
            time += Time.deltaTime;
            if (time > 1 )
            {
                time = 0;
                timeNum  -= 1;
                timer.text = timeNum.ToString();
            }
            if (timeNum <= 0 && PhotonNetwork.IsMasterClient&&!isSceneMove)
            {
                isSceneMove = true;
                Debug.Log("シーン移動");
                Debug.Log(PhotonNetwork.IsMasterClient);
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel("Game");
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && !isSceneMove)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                leftButton.interactable = false;
                isSceneMove = true;
                StartCoroutine(GameStart());
            }
        }
        if (isSceneMove)
        {
            GManager.Instance.Loading(PhotonNetwork.LevelLoadingProgress);
        }
    }
    public void SinglePlay()
    {

        SceneManager.LoadScene("Game");
    }

    public void OnlineMode()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnClickEnterRoom()
    {
        isEnterRoom = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnClickCreateRoom()
    {
        selectPanel.SetActive(true);       
    }



    public override void OnConnectedToMaster()
    {
        Debug.Log("部屋の数" + PhotonNetwork.CountOfRooms);
        Debug.Log("さーばーに接続したよ");

        PhotonNetwork.JoinLobby();
       
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        lobbyPanel.SetActive(true);
        initPanel.SetActive(false);
    }

    public override void OnLeftLobby()
    {
        initPanel.SetActive(true);
    }

    //ルームに入室後に呼び出される
    public override void OnJoinedRoom()
    {
        waitingPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        selectPanel.SetActive(false);
        timeNum = 30;
        timer.text = timeNum.ToString();
        GManager.Instance.playerNum = PhotonNetwork.CurrentRoom.Players.Count;
        playerClone = PhotonNetwork.Instantiate(GManager.Instance.CharacterInstantiate(GManager.Instance.selectedCharacterNum).name, new Vector3(-10 + (PhotonNetwork.CurrentRoom.Players.Count - 1) * 1.5f, 0.5f, -8.5f), Quaternion.Euler(0, 180, 0));
        PhotonNetwork.LocalPlayer.NickName = PhotonNetwork.CurrentRoom.Players.Count.ToString();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStageNum(GManager.Instance.stageNum);
        }
 //       Debug.Log("部屋の人数" + PhotonNetwork.CurrentRoom.Players.Count);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"ルーム参加に失敗しました: ");
        lobbyPanel.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"ルーム参加に失敗しました: ");
        lobbyPanel.SetActive(true);
    }

    public void EnterRoom()
    {
        PhotonNetwork.JoinRandomRoom(null, 0);
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        timer.text = 30.ToString();
        timeNum = 30;
        PhotonNetwork.Destroy(playerClone);
        int num = int.Parse(otherPlayer.NickName);
        if (GManager.Instance.playerNum > num)
        {
            GManager.Instance.playerNum -= 1;
            PhotonNetwork.LocalPlayer.NickName = GManager.Instance.playerNum.ToString();
        }
        playerClone = PhotonNetwork.Instantiate(GManager.Instance.CharacterInstantiate(GManager.Instance.selectedCharacterNum).name, new Vector3(-10 + (GManager.Instance.playerNum - 1) * 1.5f, 0.5f, -8.5f), Quaternion.Euler(0, 180, 0));
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
   //     Debug.Log("部屋の人数" + PhotonNetwork.CurrentRoom.PlayerCount);
        timer.text = 30.ToString();
        timeNum = 30;
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3);
        GManager.Instance.FadeIn();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }
        yield return new WaitUntil(() => PhotonNetwork.LevelLoadingProgress == 1);
        GManager.Instance.FadeOut();
    }

    public void OnClickLeftRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        waitingPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        foreach(var po in playerObj)
        {
            Destroy(po);
        }
        playerObj.Clear();
    }
}
