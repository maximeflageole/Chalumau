using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Placeable m_placeableInPlacement;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnBuildingBtnClicked(PlaceableData placeableData)
    {
        if (!CanSpawnBuilding())
        {
            return;
        }

        m_placeableInPlacement = Instantiate(placeableData.m_prefab).GetComponent<Placeable>();
    }

    private bool CanSpawnBuilding()
    {
        return m_placeableInPlacement == null;
    }

    public void OnBuildingPlaced()
    {
        m_placeableInPlacement = null;
    }
}