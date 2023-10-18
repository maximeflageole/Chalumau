using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _Instance;
    [SerializeField]
    private Vector2Int m_mapDimensions = new Vector2Int();
    [SerializeField]
    private GameObject m_tilePrefabA;
    [SerializeField]
    private GameObject m_tilePrefabB;

    [SerializeField]
    private List<Building> m_placeableObjects;

    private void Awake()
    {
        _Instance = this;
        m_placeableObjects = new List<Building>();
        InstantiateMap();
    }

    private void InstantiateMap()
    {
        for (var i = 0; i < m_mapDimensions.x; i++)
        {
            for (var j = 0; j < m_mapDimensions.y; j++)
            {
                GameObject prefab;
                ETileType type = ETileType.Count;
                if ((i + j) % 2 == 0)
                {
                    prefab = m_tilePrefabA;
                    type = ETileType.A;
                }
                else
                {
                    prefab = m_tilePrefabB;
                    type = ETileType.B;
                }
                var go = Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity, transform).gameObject;
                go.GetComponent<Tile>().SetTileType(type);

            }
        }
    }

    public bool CanPlacePlaceable(Building placeable)
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

    private static bool Collides(Building placeable1, Building placeable2)
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

    public void PlacePlaceable(Building placeable)
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