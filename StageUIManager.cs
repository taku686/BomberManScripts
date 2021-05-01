using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUIManager : MonoBehaviour
{
    public Text bombText;
    public Text speedText;
    public Text firepowerText;
    public Text heeartText;
    public Text timeText;
    public ItemManager itemManager;

    private void Start()
    {
        bombText.text = itemManager.bombCount.ToString();
        speedText.text = itemManager.speed.ToString();
        firepowerText.text = itemManager.firePower.ToString();
        heeartText.text = itemManager.heart.ToString();

    }
}
