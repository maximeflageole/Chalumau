using UnityEngine;

public class Tile : MonoBehaviour
{
    [field:SerializeField]
    public ETileType TileType { get; protected set; }

    public void SetTileType(ETileType type)
    { 
        TileType = type;
    }
}

public enum ETileType
{
    A,
    B,
    Count
}
