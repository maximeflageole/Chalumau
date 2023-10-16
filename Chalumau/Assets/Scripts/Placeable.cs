using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Placeable : MonoBehaviour
{
    [SerializeField]
    protected float m_yOffset = 1.1f;
    [SerializeField] 
    protected float m_clippingUnitValue = 1.0f;
    [SerializeField]
    protected BuildingData m_buildingData;
    [SerializeField]
    protected Material m_invalidMaterial;
    [SerializeField]
    protected Material m_validMaterial;
    [SerializeField]
    protected Material m_placedMaterial;
    protected List<Renderer> m_allRenderers = new List<Renderer>();

    [field:SerializeField]
    public ECardinalDirection Direction { get; protected set; } = ECardinalDirection.North;
    protected Vector2Int m_cornerPosition;
    protected bool m_inPlacement = true;

    [field:SerializeField]
    public List<Vector2Int> OccupiedSpace { get; protected set; } = new List<Vector2Int>();

    private void Awake()
    {
        m_allRenderers = GetComponentsInChildren<Renderer>().ToList();
        if (m_inPlacement)
        {
            SetMaterials(EPlaceableState.Valid);
            return;
        }
        SetMaterials(EPlaceableState.Placed);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_inPlacement)
        {
            UpdateBuildingLocation();
            UpdateBuildingRotation();
            UpdateOccupiedSpace();
            if (LevelManager._Instance.CanPlacePlaceable(this))
            {
                SetMaterials(EPlaceableState.Valid);
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceBuilding();
                }
            }
            else
            {
                SetMaterials(EPlaceableState.Invalid);
            }
        }
    }

    void PlaceBuilding()
    {
        m_inPlacement = false;
        OccupiedSpace = new List<Vector2Int>();
        UpdateOccupiedSpace();
        LevelManager._Instance.PlacePlaceable(this);
        GameManager.Instance.OnBuildingPlaced();
    }

    void UpdateBuildingLocation()
    {
        //TODO MF: All this should be cleaned
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Plane"); ;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, layerMask))
        {
            Vector3 clippedPoint = new Vector3(Mathf.Floor(hit.point.x),
               hit.point.y + m_yOffset,
               Mathf.Floor(hit.point.z));

            m_cornerPosition = new Vector2Int((int)clippedPoint.x, (int)clippedPoint.z);
            transform.position = clippedPoint + new Vector3(0.5f, 0, 0.5f);
        }
    }

    void UpdateBuildingRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -90, 0));
            Direction--;
            if (Direction < 0)
            {
                Direction += (int)ECardinalDirection.Count;
            }
            Direction = (ECardinalDirection)((int)Direction % (int)(ECardinalDirection.Count));
            Debug.Log(Direction.ToString());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Direction++;
            transform.Rotate(new Vector3(0, 90, 0));
            Direction = (ECardinalDirection)((int)Direction % (int)(ECardinalDirection.Count));
            Debug.Log(Direction.ToString());
        }
    }

    protected void UpdateOccupiedSpace()
    {
        OccupiedSpace.Clear();
        Vector2Int currentPos = new Vector2Int(m_cornerPosition.x, m_cornerPosition.y);
        OccupiedSpace = GetRotatedOccupiedEmplacement(currentPos);
        SetMaterials(EPlaceableState.Placed);
    }

    protected void SetMaterials(EPlaceableState state)
    {
        Material material = null;
        switch (state)
        {
            case EPlaceableState.Invalid:
                material = m_invalidMaterial;
                break;
            case EPlaceableState.Valid:
                material = m_validMaterial;
                break;
            case EPlaceableState.Placed:
                material = m_placedMaterial;
                break;
        }

        foreach (var renderer in m_allRenderers)
        {
            renderer.material = material;
        }
    }

    protected List<Vector2Int> GetRotatedOccupiedEmplacement(Vector2Int origin)
    {
        var rotatedArray = new List<Vector2Int>();

        for (var i = 0; i < m_buildingData.m_dimensions.Length; i++)
        {
            for (var j = 0; j < m_buildingData.m_dimensions[i].Array.Length; j++)
            {
                if (m_buildingData.m_dimensions[i].Array[j])
                {
                    switch (Direction)
                    {
                        case ECardinalDirection.North:
                            rotatedArray.Add(new Vector2Int(origin.x + i, origin.y + j));
                            break;
                        case ECardinalDirection.East:
                            rotatedArray.Add(new Vector2Int(origin.x + j, origin.y - i));
                            break;
                        case ECardinalDirection.South:
                            rotatedArray.Add(new Vector2Int(origin.x - i, origin.y - j));
                            break;
                        case ECardinalDirection.West:
                            rotatedArray.Add(new Vector2Int(origin.x - j, origin.y + i));
                            break;
                        case ECardinalDirection.Count:
                            break;
                    }
                }
            }
        }
        return rotatedArray;
    }
}

public enum EPlaceableState
{
    Invalid,
    Valid,
    Placed
}