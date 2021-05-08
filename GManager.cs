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
    public bool isOffLine;
    public int stageNum_OffLine;
    public int stageNum = 1;
    public int selectedCharacterNum;
    public int playerNum;
    public bool isGameStart;
    public BattleMode battleMode= BattleMode.TimeMode;
    public int heart;
    public float time;
    public int round;
    public enum BattleMode
    {
        SurvivalMode,
        TimeMode,
    }


    protected override void Awake()
    {
        base.Awake();   
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        if (isOffLine)
        {
            PhotonNetwork.OfflineMode = true;
        }
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
      
        if (!isOffLine)
        {
            if (selectedCharacterNum == 1)
            {
                PlayerCloneInstantiate(bacho_Play, startPos);
            }
            else if (selectedCharacterNum == 2)
            {
                PlayerCloneInstantiate(liube_Play, startPos);
            }
            else if (selectedCharacterNum == 3)
            {
                PlayerCloneInstantiate(tyouun_Play, startPos);
            }
            else if (selectedCharacterNum == 4)
            {
                PlayerCloneInstantiate(syuuyu_Play, startPos);
            }
            else if (selectedCharacterNum == 5)
            {
                PlayerCloneInstantiate(shibasyou_Play, startPos);
            }
            else if (selectedCharacterNum == 6)
            {
                PlayerCloneInstantiate(ryofu_Play, startPos);
            }
            else if (selectedCharacterNum == 7)
            {
                PlayerCloneInstantiate(syokatsuryou_Play, startPos);
            }
            else if (selectedCharacterNum == 8)
            {
                PlayerCloneInstantiate(sousou_Play, startPos);
            }
            else if (selectedCharacterNum == 9)
            {
                PlayerCloneInstantiate(sonsaku_Play, startPos);
            }
        }
        else
        {
            if (selectedCharacterNum == 1)
            {
                PlayerCloneInstantiate_OffLine(bacho_Play, startPos);
            }
            else if (selectedCharacterNum == 2)
            {
                PlayerCloneInstantiate_OffLine(liube_Play, startPos);
            }
            else if (selectedCharacterNum == 3)
            {
                PlayerCloneInstantiate_OffLine(tyouun_Play, startPos);
            }
            else if (selectedCharacterNum == 4)
            {
                PlayerCloneInstantiate_OffLine(syuuyu_Play, startPos);
            }
            else if (selectedCharacterNum == 5)
            {
                PlayerCloneInstantiate_OffLine(shibasyou_Play, startPos);
            }
            else if (selectedCharacterNum == 6)
            {
                PlayerCloneInstantiate_OffLine(ryofu_Play, startPos);
            }
            else if (selectedCharacterNum == 7)
            {
                PlayerCloneInstantiate_OffLine(syokatsuryou_Play, startPos);
            }
            else if (selectedCharacterNum == 8)
            {
                PlayerCloneInstantiate_OffLine(sousou_Play, startPos);
            }
            else if (selectedCharacterNum == 9)
            {
                PlayerCloneInstantiate_OffLine(sonsaku_Play, startPos);
            }
        }
    }

    private void PlayerCloneInstantiate(GameObject player,Vector3 startPos)
    {
        player = PhotonNetwork.Instantiate(player.name, startPos, player.transform.rotation);
        if (player.GetComponent<PhotonView>().IsMine)
        {
            player.GetComponent<PlayerBase>().enabled = true;
            foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
            {
                collider.enabled = true;
            }
        }
    }

    private void PlayerCloneInstantiate_OffLine(GameObject player, Vector3 startPos)
    {
        player = Instantiate(player, startPos, player.transform.rotation);
        player.GetComponent<PlayerBase_OffLine>().enabled = true;
        foreach (var collider in player.GetComponentsInChildren<BoxCollider>())
        {
            collider.enabled = true;
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
