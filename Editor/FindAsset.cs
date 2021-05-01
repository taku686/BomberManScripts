using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindAsset
{
    public string AssetName { get; }
    public string AssetPath { get; }
    public string AssetGuid { get; }

    public bool Foltout { get; set; }
    public FindAsset(string name, string path, string guid)
    {
        AssetName = name;
        AssetPath = path;
        AssetGuid = guid;
    }
}
