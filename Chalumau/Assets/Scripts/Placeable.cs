using System.Collections.Generic;
using UnityEngine;

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

    [field:SerializeField]
    public ECardinalDirection Direction { get; protected set; } = ECardinalDirection.North;
    protected Vector2Int m_cornerPosition;
    protected bool m_inPlacement = true;

    public List<Vector2Int> OccupiedSpace { get; protected set; } = new List<Vector2Int>();

    private void Awake()
    {
        if (m_inPlacement)
        {
            GetComponent<Renderer>().material = m_validMaterial;
            return;
        }
        GetComponent<Renderer>().material = m_placedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_inPlacement)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
                return;
            }
            UpdateBuildingLocation();
            UpdateBuildingRotation();
            UpdateOccupiedSpace();
            if (LevelManager._Instance.CanPlacePlaceable(this))
            {
                GetComponent<Renderer>().material = m_validMaterial;
            }
            else
            {
                GetComponent<Renderer>().material = m_invalidMaterial;
            }
        }
    }

    void PlaceBuilding()
    {
        m_inPlacement = false;
        OccupiedSpace = new List<Vector2Int>();
        UpdateOccupiedSpace();
        LevelManager._Instance.PlacePlaceable(this);
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
            transform.position = clippedPoint;
        }
    }

    void UpdateBuildingRotation()
    {
        if (Input.GetKeyDown(KeyCode.A))
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
        if (Input.GetKeyDown(KeyCode.D))
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

        for (int i = 0; i < m_buildingData.m_dimensions.Length; i++)
        {
            currentPos.y = m_cornerPosition.y;

            for (int j = 0; j < m_buildingData.m_dimensions[i].Array.Length; j++)
            {
                if (m_buildingData.m_dimensions[i].Array[j])
                {
                    OccupiedSpace.Add(new Vector2Int(currentPos.x, currentPos.y));
                }
                TranslateAccordingToDirection(ref currentPos, false, true);
            }
            TranslateAccordingToDirection(ref currentPos, true, false);
        }
        GetComponent<Renderer>().material = m_placedMaterial;
    }

    void TranslateAccordingToDirection(ref Vector2Int vector, bool x, bool y)
    {
        Vector2Int translation = new Vector2Int();
        if (x)
        {
            switch (Direction)
            {
                case ECardinalDirection.North:
                    translation += new Vector2Int(1, 0);
                    break;
                case ECardinalDirection.East:
                    translation += new Vector2Int(0, 1);
                    break;
                case ECardinalDirection.South:
                    translation += new Vector2Int(-1, 0);
                    break;
                case ECardinalDirection.West:
                    translation += new Vector2Int(0, -1);
                    break;
                case ECardinalDirection.Count:
                    break;
            }
        }
        if (y)
        {
            switch (Direction)
            {
                case ECardinalDirection.North:
                    translation += new Vector2Int(0, 1);
                    break;
                case ECardinalDirection.East:
                    translation += new Vector2Int(1, 0);
                    break;
                case ECardinalDirection.South:
                    translation += new Vector2Int(0, -1);
                    break;
                case ECardinalDirection.West:
                    translation += new Vector2Int(-1, 0);
                    break;
                case ECardinalDirection.Count:
                    break;
            }
        }
        vector += translation;
    }
}
