using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListEntry : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text roomNameameLabel = default;
    [SerializeField]
    private Text recruitmentLabel = default;
    [SerializeField]
    private Text playerCounter = default;
    private RectTransform rectTransform;
    private Button button;
    private string roomName;
    private UIManager uIManager;

    private void Awake()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
       
    }

    private void Start()
    {
        // リスト要素がクリックされたら、対応したルーム名のルームに参加する
        button.onClick.AddListener(() => uIManager.OnPlayerSelectPanel_Click(roomName));
    }

    private void OnClickAction()
    {
        //  PhotonNetwork.JoinRoom(roomName);
     
    }

    public void Activate(RoomInfo info)
    {
        roomName = info.Name;

        //   roomNameameLabel.text = (string)info.CustomProperties["DisplayName"];
        roomNameameLabel.text = (string)info.Name;
        if (info.IsOpen)
        {
            recruitmentLabel.text = "募集中";
        }
        else
        {
            recruitmentLabel.text = "プレイ中";
        }
       
        playerCounter.text = $"{info.PlayerCount}/{info.MaxPlayers}";
        //  playerCounter.SetText("{0}/{1}", info.PlayerCount, info.MaxPlayers);
        // ルームの参加人数が満員でない時だけ、クリックできるようにする
        button.interactable = (info.PlayerCount < info.MaxPlayers);

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public RoomListEntry SetAsLastSibling()
    {
        rectTransform.SetAsLastSibling();
        return this;
    }
}
