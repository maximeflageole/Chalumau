using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "New LevelData")]
public class LevelData : ScriptableObject
{
    public List<LevelSupply> levelSupplies = new List<LevelSupply>();
}

[System.Serializable]
public struct LevelSupply
{
    public GameObject SupplySource;
    public Vector2Int CornerLocation;
    public ECardinalDirection Orientation;
}