using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "Placeable", order = 1)]
public class PlaceableData : ScriptableObject
{
    public BoolArray[] m_dimensions = new BoolArray[5];
    public GameObject m_prefab;
    [field: SerializeField]
    public List<SupplyQty> SuppliesOutput { get; protected set; } = new List<SupplyQty>();
    [field: SerializeField]
    public List<SupplyQty> SuppliesInputs { get; protected set; } = new List<SupplyQty>();
}

[System.Serializable]
public struct BoolArray
{
    public BoolArray(int size) { Array = new bool[size]; }
    public bool[] Array;
}

[System.Serializable]
public struct SupplyQty
{
    public ESupplyType SupplyType;
    public int Qty;
}