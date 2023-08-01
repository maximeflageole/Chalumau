using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField] float m_clippingUnitValue = 1.0f;
    bool m_inPlacement = true;

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
        }
    }

    void PlaceBuilding()
    {
        m_inPlacement = false;
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
               hit.point.y,
               Mathf.Floor(hit.point.z));

            transform.position = clippedPoint;
        }
    }

    void UpdateBuildingRotation()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, -90, 0));
        }
    }
}
