using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GManager : SingletonMonoBehaviour<GManager>
{
    [SerializeField] GameObject bacho;
    [SerializeField] GameObject liube;
    [SerializeField] GameObject tyouun;
    [SerializeField] GameObject syuuyu;
    [SerializeField] GameObject shibasyou;
    [SerializeField] GameObject ryofu;
    [SerializeField] GameObject syokatsuryou;
    [SerializeField] GameObject sousou;
    [SerializeField] GameObject sonsaku;
    [SerializeField] GameObject bacho_Play;
    [SerializeField] GameObject liube_Play;
    [SerializeField] GameObject tyouun_Play;
    [SerializeField] GameObject syuuyu_Play;
    [SerializeField] GameObject shibasyou_Play;
    [SerializeField] GameObject ryofu_Play;
    [SerializeField] GameObject syokatsuryou_Play;
    [SerializeField] GameObject sousou_Play;
    [SerializeField] GameObject sonsaku_Play;
    [SerializeField] GameObject sceneImage;
    [SerializeField] Slider slider;
    public int stageNum = 1;
    public int selectedCharacterNum;
    public int playerNum;
    public bool isGameStart;

    protected override void Awake()
    {
        base.Awake();   
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public GameObject CharacterInstantiate(int characterNum)
    {
        if (characterNum == 1)
        {
            return bacho;
        }
        else if (characterNum == 2)
        {
            return liube;
        }
        else if (characterNum == 3)
        {
            return tyouun;
        }
        else if (characterNum == 4)
        {
            return syuuyu;
        }
        else if (characterNum == 5)
        {
            return shibasyou;
        }
        else if (characterNum == 6)
        {
            return ryofu;
        }
        else if (characterNum == 7)
        {
            return syokatsuryou;
        }
        else if (characterNum == 8)
        {
            return sousou;
        }
        else if (characterNum == 9)
        {
            return sonsaku;
        }
        else
        {
            return bacho;
        }
    }

    // Start is called before the first frame update
    public void PlayerInstantiate()
    {
        GameObject player;
        Vector3 startPos = Vector3.zero;
        if (playerNum == 1)
        {
            startPos = new Vector3(-1, 0, 10);
        }
        else if (playerNum == 2)
        {
            startPos = new Vector3(10, 0, 10);
        }
        else if (playerNum == 3)
        {
            startPos = new Vector3(-1, 0, 0);
        }
        else if (playerNum == 4)
        {
            startPos = new Vector3(10, 0, 0);
        }

        if (selectedCharacterNum == 1)
        {
            Debug.Log("プレイヤー１生成");
            player = PhotonNetwork.Instantiate(bacho_Play.name, startPos, bacho_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 2)
        {
            player = PhotonNetwork.Instantiate(liube_Play.name, startPos, liube_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 3)
        {
            player = PhotonNetwork.Instantiate(tyouun_Play.name, startPos, tyouun_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 4)
        {
            player = PhotonNetwork.Instantiate(syuuyu_Play.name, startPos, syuuyu_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 5)
        {
            player = PhotonNetwork.Instantiate(shibasyou_Play.name, startPos, shibasyou_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 6)
        {
            player = PhotonNetwork.Instantiate(ryofu_Play.name, startPos, ryofu_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 7)
        {
            player = PhotonNetwork.Instantiate(syokatsuryou_Play.name, startPos, syokatsuryou_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 8)
        {
            player = PhotonNetwork.Instantiate(sousou_Play.name, startPos, sousou_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
        else if (selectedCharacterNum == 9)
        {
            player = PhotonNetwork.Instantiate(sonsaku_Play.name, startPos, sonsaku_Play.transform.rotation);
            if (player.GetComponent<PhotonView>().IsMine)
            {
                player.GetComponent<PlayerBase>().enabled = true;
                foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
                {
                    collider.enabled = true;
                }
            }
        }
    }

   

    public void Loading(float value)
    {
        slider.value = value;
    }

    public void FadeIn()
    {
        sceneImage.SetActive(true);
    }

    public void FadeOut()
    {
        sceneImage.SetActive(false);
    }
}
