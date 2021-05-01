using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindReferenceAsset :EditorWindow
{
    private const string findType = @"t:scene t:prefab t:timelineAsset t:animatorcontroller t:Material";
    private static Dictionary<FindAsset, List<FindAsset>> result = new Dictionary<FindAsset, List<FindAsset>>();
    private Vector2 scrollPosition = Vector2.zero;
    [MenuItem("Assets/参照を探す", false)]
    public static void FindAssets()
    {

        Object[] selects = Selection.objects;
        if (selects == null || selects.Length == 0)
        {
            Debug.Log("There are no select objects");
            return;
        }
        result.Clear();

        List<FindAsset> selectAssets = new List<FindAsset>();
        foreach (var select in selects)
        {
            var path = AssetDatabase.GetAssetPath(select);
            var guid = AssetDatabase.AssetPathToGUID(path);
            selectAssets.Add(new FindAsset(select.name, path, guid));
        }

        var ids = AssetDatabase.FindAssets(findType);
        //プロジェクト内にある検索対象のアセットをすべて取得する
        foreach (string guid in ids)
        {
            SetFindAssets(guid, selectAssets);
        }

        GetWindow<FindReferenceAsset>();
    }

    private static void SetFindAssets(string guid, List<FindAsset> selectAssets)
    {
        var path = AssetDatabase.GUIDToAssetPath(guid);
        //対象のアセットと
        //依存関係にあるアセットをすべて取得する
        var deps = AssetDatabase.GetDependencies(path);
        foreach (string depPath in deps)
        {
            if (path == depPath) continue;
            var dependGuid = GetGuid(depPath);
            int count = selectAssets.Count;
            for (int i = 0; i < count; i++)
            {
                var asset = selectAssets[i];
                if (!result.ContainsKey(asset))
                {
                    result.Add(asset, new List<FindAsset>());
                }
                //選択したアセットと同じGUIDであれば
                //参照されているということになる
                if (dependGuid == asset.AssetGuid)
                {
                    var hitName = GetAssetName(path);
                    var hit = new FindAsset(hitName, path, guid);
                    result[asset].Add(hit);
                }
            }
        }
    }

    //PathからGUIDを取得する
    private static string GetGuid(string path)
    {
        return AssetDatabase.AssetPathToGUID(path);
    }

    private static string GetAssetName(string path)
    {
        var unityObject = AssetDatabase.LoadMainAssetAtPath(path);
        if(unityObject == null)
        {
            return string.Empty;
        }

        return unityObject.name;
    }

    private void OnGUI()
    {
        if (result == null || result.Count == 0) return;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (KeyValuePair<FindAsset, List<FindAsset>> pair in result)
        {
            int referenceCount = pair.Value == null ? 0 : pair.Value.Count;
            if (pair.Key.Foltout = EditorGUILayout.Foldout(pair.Key.Foltout, pair.Key.AssetName + " : " + referenceCount + "個"))
            {
                foreach (var item in pair.Value)
                {

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);

                    EditorGUIUtility.SetIconSize(Vector2.one * 16);

                    Object target = AssetDatabase.LoadAssetAtPath<Object>(item.AssetPath);
                    //アセットがPrefabなのかSceneなのか判別付きやすくするためにアイコンの表示
                    GUIContent guiContent = new GUIContent(
                        item.AssetName,
                        EditorGUIUtility.ObjectContent(target, target.GetType()).image
                    );

                    if (GUILayout.Button(guiContent, "Label"))
                    {
                        Selection.objects = new[] { target };
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
