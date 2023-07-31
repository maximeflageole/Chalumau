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
            UpdateBuildingPlacement();
        }
    }

    void PlaceBuilding()
    {
        m_inPlacement = false;
    }

    void UpdateBuildingPlacement()
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
}
