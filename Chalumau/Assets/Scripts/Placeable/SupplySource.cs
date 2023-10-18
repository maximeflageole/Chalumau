using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplySource : Placeable
{
    [field:SerializeField]
    public List<ESupplyType> SuppliesList { get; protected set; } = new List<ESupplyType>();


}

public enum ESupplyType
{
    Forest,
    Grain,
    Stone,
    Count
}