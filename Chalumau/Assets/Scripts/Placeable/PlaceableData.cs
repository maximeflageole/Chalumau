using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "Placeable", order = 1)]
public class PlaceableData : ScriptableObject
{
    public BoolArray[] m_dimensions = new BoolArray[5];
    public GameObject m_prefab;
}

[System.Serializable]
public struct BoolArray
{
    public BoolArray(int size) { Array = new bool[size]; }
    public bool[] Array;
}