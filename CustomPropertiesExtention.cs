using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class CustomPropertiesExtention 
{
    private const string playerNumKey = "pNum";
    private const string characterNumKey = "cNum";
    private const string playerNameKey = "pName";
    private const string stageNumKey = "sNum";
    private const string timeNumKey = "tNum";
    private static readonly Hashtable hashtable = new Hashtable();

    public static void SetPlayerNum(this Player player, int actorNum)
    {
        hashtable[playerNumKey] = actorNum;
        player.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    public static void SetCharacterNum(this Player player, int characterNum)
    {
   //     Debug.Log("キャラクター設定");
        hashtable[characterNumKey] = characterNum;
        player.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    // プレイヤーのメッセージを設定する
    public static void SetPlayerName(this Player player, string message)
    {
        hashtable[playerNameKey] = message;
        player.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    public static void SetStageNum(this Room room, int stageNum)
    {
        hashtable[stageNumKey] = stageNum;
        room.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    public static void SetTimeUpdate(this Room room,float time)
    {
        hashtable[timeNumKey] = time;
        room.SetCustomProperties(hashtable);
        hashtable.Clear();
    }

    public static int GetPlayerNum(this Player player)
    {
        return (player.CustomProperties[playerNumKey] is int actorNum) ? actorNum : 0;
    }

    public static int GetCharacterNum(this Player player)
    {
        Debug.Log("キャラクターの番号");
        return (player.CustomProperties[characterNumKey] is int characterNum) ? characterNum : 0;
    }

    public static string GetPlayerName(this Player player)
    {
        return (player.CustomProperties[playerNameKey] is string name) ? name : string.Empty;
    }

    public static int GetStageNum(this Room room)
    {
        return (room.CustomProperties[stageNumKey] is int stageNum) ? stageNum : 1;
    }

    public static float GetTimeUpdate(this Room room)
    {
        return (room.CustomProperties[timeNumKey] is float time) ? time : 0;
    }
}
