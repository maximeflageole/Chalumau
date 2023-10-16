using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Placeable m_buildingInPlacement;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void OnBuildingBtnClicked(BuildingData buildingData)
    {
        if (!CanSpawnBuilding())
        {
            return;
        }

        m_buildingInPlacement = Instantiate(buildingData.m_prefab).GetComponent<Placeable>();
    }

    private bool CanSpawnBuilding()
    {
        return m_buildingInPlacement == null;
    }

    public void OnBuildingPlaced()
    {
        m_buildingInPlacement = null;
    }
}