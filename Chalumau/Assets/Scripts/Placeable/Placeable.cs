using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField]
    protected float m_yOffset = 1.1f;
    [SerializeField]
    protected Material m_placedMaterial;

    [field: SerializeField]
    public ECardinalDirection Direction { get; protected set; } = ECardinalDirection.North;
    protected Vector2Int m_cornerPosition;

    [field: SerializeField]
    public List<Vector2Int> OccupiedSpaces { get; protected set; } = new List<Vector2Int>();
}
