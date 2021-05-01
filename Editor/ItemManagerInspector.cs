using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapTileGridCreator.Utilities;
/*
[CustomEditor(typeof(ItemManager))]
public class ItemManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ItemManager itemManager = (ItemManager)target;
        DrawUILine(Color.gray);
        EditorGUILayout.LabelField("ボム切り替え");
        DrawUILine(Color.gray);
        using (new GUILayout.VerticalScope())
        {
            itemManager.isNormal = GUILayout.Toggle(itemManager.isNormal,"ノーマルボム");
            itemManager.isPenetration = GUILayout.Toggle(itemManager.isPenetration, "貫通ボム");
            itemManager.isBounce = GUILayout.Toggle(itemManager.isBounce, "バウンスボム");
            itemManager.isDiffuse = GUILayout.Toggle(itemManager.isDiffuse, "拡散ボム");
        }
        DrawUILine(Color.gray);
        EditorGUILayout.LabelField("ステータス");
        DrawUILine(Color.gray);
        using (new GUILayout.VerticalScope())
        {
            itemManager.bombCount = EditorGUILayout.IntField("ボム数", itemManager.bombCount);
            itemManager.speed = EditorGUILayout.IntField("スピード", itemManager.speed);
            itemManager.firePower = EditorGUILayout.IntField("火力", itemManager.firePower);
            itemManager.heart = EditorGUILayout.IntField("ハート", itemManager.heart);
        }
        DrawUILine(Color.gray);
        EditorGUILayout.LabelField("特殊能力");
        DrawUILine(Color.gray);
        using (new GUILayout.VerticalScope())
        {
            itemManager.isKick = GUILayout.Toggle(itemManager.isKick, "キック");
            itemManager.isThrow= GUILayout.Toggle(itemManager.isThrow, "投げる");
            itemManager.isBarrier = GUILayout.Toggle(itemManager.isBarrier, "バリアー");
            itemManager.isReflection = GUILayout.Toggle(itemManager.isReflection, "リフレクター");
            itemManager.isJump = GUILayout.Toggle(itemManager.isJump, "ジャンプ");
        }
    }

   private void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }


}
*/
