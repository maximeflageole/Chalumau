using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    public GameObject m_prefab;
    public BoolArray[] m_dimensions = new BoolArray[5];
}

[System.Serializable]
public struct BoolArray
{
    public BoolArray(int size) { Array = new bool[size]; }
    public bool[] Array;
}