using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private BuildingData m_buildingData;


    private Placeable m_buildingInPlacement;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void OnClick()
    {
        if (!CanSpawnBuilding())
        {
            return;
        }

        m_buildingInPlacement = Instantiate(m_buildingData.m_prefab).GetComponent<Placeable>();
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