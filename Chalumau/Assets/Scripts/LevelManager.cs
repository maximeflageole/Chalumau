using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _Instance;
    [SerializeField]
    private Vector2Int m_mapDimensions = new Vector2Int();

    [SerializeField]
    private List<Placeable> m_placeableObjects;

    private void Awake()
    {
        _Instance = this;
        m_placeableObjects = new List<Placeable>();
    }

    public bool CanPlacePlaceable(Placeable placeable)
    {
        if (!InLevelDimensions(placeable.OccupiedSpaces))
        {
            return false;
        }
        foreach (var placedObject in m_placeableObjects)
        {
            if (Collides(placedObject, placeable))
            {
                return false;
            }
        }
        return true;
    }

    private static bool Collides(Placeable placeable1, Placeable placeable2)
    {
        foreach (var space in placeable1.OccupiedSpaces)
        {
            foreach (var space2 in placeable2.OccupiedSpaces)
            {
                Debug.Log("space1 = " + space + " space2: " + space2);
                if (space == space2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void PlacePlaceable(Placeable placeable)
    {
        m_placeableObjects.Add(placeable);
    }

    public bool InLevelDimensions(List<Vector2Int> occupiedPositions)
    {
        foreach (var pos in occupiedPositions)
        {
            if (pos.x < 0 || pos.x >= m_mapDimensions.x
                || pos.y < 0 || pos.y >= m_mapDimensions.y)
            {
                return false;
            }
        }
        return true;
    }
}

public enum ECardinalDirection
{
    North,
    East,
    South,
    West,
    Count
}