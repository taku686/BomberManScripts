using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(DataStage))]
public class DataStageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get this Stage-Data
        DataStage dataStage = target as DataStage;

        // Find "Stage" Object in Hierarchy-Window
        var stageObject = GameObject.Find("Stage");

        // Get Objects with Stage-Components("Item", Enemy", etc) in "Stage" Object
        var floorAs = stageObject.GetComponentsInChildren<FloorA>();
        var floorBs = stageObject.GetComponentsInChildren<FloorB>();
        var characters = stageObject.GetComponentsInChildren<StageCharacter>();

        if (GUILayout.Button("Save"))
        {
            // Overwrite Item-Data (Scriptable Object)
            dataStage.floorA = new DataFloorA[floorAs.Length];
            for (int i = 0; i < floorAs.Length; i++)
            {
                dataStage.floorA[i] = new DataFloorA();
                dataStage.floorA[i].name = floorAs[i].gameObject.name;
                dataStage.floorA[i].localPosition = floorAs[i].transform.localPosition;
                dataStage.floorA[i].localRotation = floorAs[i].transform.localRotation;
                dataStage.floorA[i].localScale = floorAs[i].transform.localScale;
                // and more Item-Component unique params
            }

            // Overwrite Item-Data (Scriptable Object)
            dataStage.floorB = new DataFloorB[floorBs.Length];
            for (int i = 0; i < floorBs.Length; i++)
            {
                dataStage.floorB[i] = new DataFloorB();
                dataStage.floorB[i].name = floorBs[i].gameObject.name;
                dataStage.floorB[i].localPosition = floorBs[i].transform.localPosition;
                dataStage.floorB[i].localRotation = floorBs[i].transform.localRotation;
                dataStage.floorB[i].localScale = floorBs[i].transform.localScale;
                // and more Item-Component unique params
            }

            // Overwrite Enemy-Data (Scriptable Object)
            dataStage.characters = new DataCharacter[characters.Length];
            for (int i = 0; i < characters.Length; i++)
            {
                dataStage.characters[i] = new DataCharacter();
                dataStage.characters[i].name = characters[i].gameObject.name;
                dataStage.characters[i].localPosition = characters[i].transform.localPosition;
                dataStage.characters[i].localRotation = characters[i].transform.localRotation;
                dataStage.characters[i].localScale = characters[i].transform.localScale;
                // and more Enemy-Component unique params
            }
        }

        if (GUILayout.Button("Load"))
        {
            // Clear Stage-Objects in Hierarchy
            for (int i = floorAs.Length - 1; i >= 0; i--)
                DestroyImmediate(floorAs[i].gameObject);
            for (int i = floorBs.Length - 1; i >= 0; i--)
                DestroyImmediate(floorBs[i].gameObject);
            for (int i = characters.Length - 1; i >= 0; i--)
                DestroyImmediate(characters[i].gameObject);

            // Instantiate Stage-Enemies
            for (int i = 0; i < dataStage.characters.Length; i++)
            {
                var obj = Instantiate(Resources.Load("StageObject/Character")) as GameObject;
                obj.name = dataStage.characters[i].name;
                obj.transform.SetParent(stageObject.transform);
                obj.transform.localPosition = dataStage.characters[i].localPosition;
                obj.transform.localRotation = dataStage.characters[i].localRotation;
                obj.transform.localScale = dataStage.characters[i].localScale;
            }

            // Instantiate Stage-Items
            for (int i = 0; i < dataStage.floorA.Length; i++)
            {
                var obj = Instantiate(Resources.Load("StageObject/FloorA")) as GameObject;
                obj.name = dataStage.floorA[i].name;
                obj.transform.SetParent(stageObject.transform);
                obj.transform.localPosition = dataStage.floorA[i].localPosition;
                obj.transform.localRotation = dataStage.floorA[i].localRotation;
                obj.transform.localScale = dataStage.floorA[i].localScale;
            }

            // Instantiate Stage-Items
            for (int i = 0; i < dataStage.floorB.Length; i++)
            {
                var obj = Instantiate(Resources.Load("StageObject/FloorB")) as GameObject;
                obj.name = dataStage.floorB[i].name;
                obj.transform.SetParent(stageObject.transform);
                obj.transform.localPosition = dataStage.floorB[i].localPosition;
                obj.transform.localRotation = dataStage.floorB[i].localRotation;
                obj.transform.localScale = dataStage.floorB[i].localScale;
            }
        }

        DrawDefaultInspector();
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }
}
