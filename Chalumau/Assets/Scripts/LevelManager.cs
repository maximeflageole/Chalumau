using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _Instance;

    [SerializeField]
    private List<Placeable> m_placeableObjects;

    private void Awake()
    {
        _Instance = this;
        m_placeableObjects = new List<Placeable>();
    }

    public bool CanPlacePlaceable(Placeable placeable)
    {
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
        foreach (var space in placeable1.OccupiedSpace)
        {
            foreach (var space2 in placeable2.OccupiedSpace)
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
}

public enum ECardinalDirection
{
    North,
    East,
    South,
    West,
    Count
}