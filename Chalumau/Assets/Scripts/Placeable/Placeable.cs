using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Placeable : MonoBehaviour
{
    [SerializeField]
    protected bool m_inPlacement = true;

    [SerializeField]
    protected float m_yOffset = 1.1f;
    [SerializeField]
    protected float m_clippingUnitValue = 1.0f;
    [SerializeField]
    protected Material m_invalidMaterial;
    [SerializeField]
    protected Material m_validMaterial;
    protected List<Renderer> m_allRenderers = new List<Renderer>();

    [SerializeField]
    protected Material m_placedMaterial;

    [SerializeField]
    public PlaceableData m_data;
    [field: SerializeField]
    public ECardinalDirection Direction { get; protected set; } = ECardinalDirection.North;
    protected Vector2Int m_cornerPosition;

    [field: SerializeField]
    public List<Vector2Int> OccupiedSpaces { get; protected set; } = new List<Vector2Int>();

    [field: SerializeField]
    public List<ESupplyType> SuppliesList { get; protected set; } = new List<ESupplyType>();

    protected void Awake()
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
            //UpdateBuildingRotation();
            UpdateOccupiedSpace();
            if (CanPlace())
            {
                SetMaterials(EPlaceableState.Valid);
                if (Input.GetMouseButtonDown(0))
                {
                    Place();
                }
            }
            else
            {
                SetMaterials(EPlaceableState.Invalid);
            }
        }
    }

    public virtual void Place()
    {
        OccupiedSpaces = new List<Vector2Int>();
        UpdateOccupiedSpace();
        LevelManager._Instance.PlacePlaceable(this);
        transform.Translate(Vector3.up * m_yOffset);
        m_inPlacement = false;
        GameManager.Instance.OnBuildingPlaced();
        SetMaterials(EPlaceableState.Placed);
    }

    protected void UpdateBuildingLocation()
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

    protected virtual bool CanPlace()
    {
        if (LevelManager._Instance.CanPlacePlaceable(this))
        {
            return true;
        }
        return false;
    }

    protected virtual void UpdateOccupiedSpace()
    {
        OccupiedSpaces.Clear();
        Vector2Int currentPos = new Vector2Int(m_cornerPosition.x, m_cornerPosition.y);
        OccupiedSpaces = GetRotatedOccupiedEmplacement(currentPos);
    }

    void UpdateBuildingRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -90, 0));
            Direction--;
            if (Direction< 0)
            {
                Direction += (int) ECardinalDirection.Count;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Direction++;
            transform.Rotate(new Vector3(0, 90, 0));
        }
        Direction = (ECardinalDirection)((int)Direction % (int)(ECardinalDirection.Count));
        Debug.Log(Direction.ToString());
    }

    protected List<Vector2Int> GetRotatedOccupiedEmplacement(Vector2Int origin)
    {
        var rotatedArray = new List<Vector2Int>();

        for (var i = 0; i < m_data.m_dimensions.Length; i++)
        {
            for (var j = 0; j < m_data.m_dimensions[i].Array.Length; j++)
            {
                if (m_data.m_dimensions[i].Array[j])
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

    protected void SetMaterials(EPlaceableState state)
    {
        Material material = null;
        switch (state)
        {
            case EPlaceableState.Invalid:
                if (m_invalidMaterial == null) return;
                material = m_invalidMaterial;
                break;
            case EPlaceableState.Valid:
                if (m_validMaterial == null) return;
                material = m_validMaterial;
                break;
            case EPlaceableState.Placed:
                if (m_placedMaterial == null) return;
                material = m_placedMaterial;
                break;
        }

        foreach (var renderer in m_allRenderers)
        {
            renderer.material = material;
        }
    }
}

public enum EPlaceableState
{
    Invalid,
    Valid,
    Placed
}

public enum ESupplyType
{
    Forest,
    Grain,
    Stone,
    Count
}