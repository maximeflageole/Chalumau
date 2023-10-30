using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _Instance;
    [SerializeField]
    private Vector2Int m_mapDimensions = new Vector2Int();
    [SerializeField]
    private LevelData m_data;
    [SerializeField]
    private GameObject m_tilePrefabA;
    [SerializeField]
    private GameObject m_tilePrefabB;

    [SerializeField]
    private List<Placeable> m_placeableObjects;

    [SerializeField]
    private TextMeshProUGUI m_scoreText;
    private uint m_currentScore = 0;

    private void Awake()
    {
        _Instance = this;
        m_placeableObjects = new List<Placeable>();
        InstantiateMap();
    }

    private void Update()
    {
        m_scoreText.text = m_currentScore.ToString() + " / " + m_data.ScoreRequired.ToString();
    }

    private void InstantiateMap()
    {
        foreach (var supply in m_data.levelSupplies)
        {
            var go = Instantiate(supply.SupplySource, new Vector3(supply.CornerLocation.x, 0, supply.CornerLocation.y),
                Quaternion.identity, transform).gameObject;
            var placeable = go.GetComponent<Placeable>();
            placeable.PlaceAtLocation(supply.CornerLocation);
        }

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
        if (!HasSuppliesRequirementsMet(placeable))
        {
            return false;
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

    //TODO MF: Placeables should be put in a dictionary for instant access instead of this
    private bool TryGetPlaceableAtLocation(Vector2Int location, ref Placeable placeable)
    {
        foreach (var placeableObj in m_placeableObjects)
        {
            if (placeableObj.IsOnLocation(location))
            {
                placeable = placeableObj;
                return true;
            }
        }
        return false;
    }

    public void PlacePlaceable(Placeable placeable)
    {
        m_placeableObjects.Add(placeable);
        m_currentScore += placeable.m_data.m_scoreValue;
        ConsumeAdjacentResources(placeable);
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

    private bool HasSuppliesRequirementsMet(Placeable placeable)
    {
        if (placeable.m_data.SuppliesInputs.Count == 0) return true;

        var adjacentPlaceables = GetAdjacentPlaceables(placeable);

        foreach (var requirement in placeable.m_data.SuppliesInputs)
        {
            int requirementCount = requirement.Qty;
            foreach (var element in adjacentPlaceables)
            {
                foreach (var supply in element.SuppliesDict)
                {
                    if (supply.Key == requirement.SupplyType)
                    {
                        requirementCount--;
                    }
                }
            }
            if (requirementCount > 0)
            {
                return false;
            }
        }
        return true;
    }

    private void ConsumeAdjacentResources(Placeable placeable)
    {
        //Assuming you already verified with HasSuppliesRequirementsMet
        if (placeable.m_data.SuppliesInputs.Count == 0) return;

        var adjacentPlaceables = GetAdjacentPlaceables(placeable);

        foreach (var requirement in placeable.m_data.SuppliesInputs)
        {
            foreach (var adjacentPlaceable in adjacentPlaceables)
            {
                adjacentPlaceable.ConsumeResources(requirement.SupplyType);
            }
        }
        foreach (var adjacentPlaceable in adjacentPlaceables)
        {
            adjacentPlaceable.RefreshSuppliesDictionary();
        }
    }

    private HashSet<Placeable> GetAdjacentPlaceables(Placeable placeable)
    {
        HashSet<Placeable> adjacentPlaceables = new HashSet<Placeable>();

        foreach (var locationOccupied in placeable.OccupiedSpaces)
        {
            adjacentPlaceables.AddRange(GetAdjacentPlaceablesAtLocation(locationOccupied, placeable));
        }
        return adjacentPlaceables;
    }

    private HashSet<Placeable> GetAdjacentPlaceablesAtLocation(Vector2Int location, Placeable originalPlaceable = null)
    {
        HashSet<Placeable> placeables = new HashSet<Placeable>();
        Placeable placeable = null;

        if (TryGetPlaceableAtLocation(new Vector2Int(location.x + 1, location.y), ref placeable))
        {
            if (originalPlaceable != placeable)
                placeables.Add(placeable);
        }
        if (TryGetPlaceableAtLocation(new Vector2Int(location.x - 1, location.y), ref placeable))
        {
            if (originalPlaceable != placeable)
                placeables.Add(placeable);
        }
        if (TryGetPlaceableAtLocation(new Vector2Int(location.x, location.y + 1), ref placeable))
        {
            if (originalPlaceable != placeable)
                placeables.Add(placeable);
        }
        if (TryGetPlaceableAtLocation(new Vector2Int(location.x, location.y - 1), ref placeable))
        {
            if (originalPlaceable != placeable)
                placeables.Add(placeable);
        }

        return placeables;
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