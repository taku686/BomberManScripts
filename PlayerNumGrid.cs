using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNumGrid : MonoBehaviour
{
    public void OnClickPlayerNum()
    {
        for(byte i=2; i < 5; i++)
        {
            if (GetComponentInChildren<Text>().text == i.ToString())
            {
                Debug.Log($"PlayerNum{i}");
                UIManager.playerNum = i;
            }
        }
    }

    public void OnClickStageSelect()
    {
        for(int i = 1; i < 5; i++)
        {
            if (gameObject.name == i.ToString())
            {
                Debug.Log($"Stage{i}");
                PhotonManager.stageNumber = i;
            }
        }
    }

    public void OnClickPlayerSelect()
    {
        for (int i = 1; i < 10; i++)
        {
            if (GetComponentInChildren<Text>().text == i.ToString())
            {
                Debug.Log($"Stage{i}");
               GManager.Instance.selectedCharacterNum = i;
            }
        }
    }
}
