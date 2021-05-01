﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListView : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomListEntry roomListEntryPrefab = default; // RoomListEntryのPrefabの参照
    [SerializeField]
    private GameObject content;
    private ScrollRect scrollRect;
    private Dictionary<string, RoomListEntry> activeEntries = new Dictionary<string, RoomListEntry>();
    private Stack<RoomListEntry> inactiveEntries = new Stack<RoomListEntry>();

    private void Awake()
    {
 //       Debug.Log("Scrollrectゲット");
        scrollRect = GetComponent<ScrollRect>();
    }

    // ルームリストが更新された時に呼ばれるコールバック
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            RoomListEntry entry;
            if (activeEntries.TryGetValue(info.Name, out entry))
            {
                if (!info.RemovedFromList)
                {
                    // リスト要素を更新する
                    entry.Activate(info);
                }
                else
                {
                    // リスト要素を削除する
                    activeEntries.Remove(info.Name);
                    entry.Deactivate();
                    inactiveEntries.Push(entry);
                }
            }
            else if (!info.RemovedFromList)
            {
                // リスト要素を追加する
                entry = (inactiveEntries.Count > 0)
                    ? inactiveEntries.Pop().SetAsLastSibling()
                    : Instantiate(roomListEntryPrefab, content.transform);
                entry.Activate(info);
                activeEntries.Add(info.Name, entry);
            }
        }
    }
}
