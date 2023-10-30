using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private BuildingButtons m_buttonSelected;
    private Placeable m_placeableInPlacement;
    [SerializeField]
    private HandPanel m_handPanel;
    [SerializeField]
    private PlaceablesPile m_drawPile;
    [SerializeField]
    private PlaceablesPile m_discardPile;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuildingPlacement();
        }
    }

    private void CancelBuildingPlacement()
    {
        if (m_placeableInPlacement == null)
        {
            return;
        }
        Destroy(m_placeableInPlacement.gameObject);
        m_placeableInPlacement = null;
        m_buttonSelected = null;
    }

    public void OnBuildingBtnClicked(BuildingButtons btn, PlaceableData placeableData)
    {
        if (!CanSpawnBuilding())
        {
            return;
        }

        m_buttonSelected = btn;
        m_placeableInPlacement = Instantiate(placeableData.m_prefab).GetComponent<Placeable>();
    }

    private bool CanSpawnBuilding()
    {
        return m_placeableInPlacement == null;
    }

    public void OnBuildingPlaced()
    {
        if (m_buttonSelected != null)
        {
            m_discardPile.AddCard(m_placeableInPlacement.m_data);
            Destroy(m_buttonSelected.gameObject);
            m_handPanel.Draw();
        }
        m_buttonSelected = null;
        m_placeableInPlacement = null;
    }

    public void ShuffleDiscardIntoDrawPile()
    {
        PlaceablesPile.TransferAllCards(m_discardPile, m_drawPile, true);
    }
}