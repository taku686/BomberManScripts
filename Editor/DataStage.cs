using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage Data")]
public class DataStage : ScriptableObject
{
    public GameObject[] prefabs;
    public DataCharacter[] characters;
    public DataFloorA[] floorA;
    public DataFloorB[] floorB;
}

[System.Serializable]
public class DataCharacter
{
    public string name;
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
}

[System.Serializable]
public class DataFloorA
{
    public string name;
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
}

[System.Serializable]
public class DataFloorB
{
    public string name;
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
}


