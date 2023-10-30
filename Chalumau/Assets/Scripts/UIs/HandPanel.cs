using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPanel : MonoBehaviour
{
    [SerializeField]
    private PlaceablesPile m_drawPile;
    [SerializeField]
    private GameObject m_buildingButtonPrefab;
    [SerializeField]
    List<PlaceableData> m_placeablesData = new List<PlaceableData>();

    private void Start()
    {
        m_drawPile.Shuffle();

        for (var i = 0; i < 5; i++)
        {
            Draw();
        }
    }

    public void Draw()
    {
        var data = m_drawPile.DrawCard();
        if (data == null) return;

        m_placeablesData.Add(data);
        var buildingButton = Instantiate(m_buildingButtonPrefab, transform).GetComponent<BuildingButtons>();
        buildingButton.SetData(data);
    }

    public void RemoveItem(int index)
    {
        m_placeablesData.RemoveAt(index);
    }
}
