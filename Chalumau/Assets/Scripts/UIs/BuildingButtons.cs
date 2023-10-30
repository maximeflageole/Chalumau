using TMPro;
using UnityEngine;

public class BuildingButtons : MonoBehaviour
{
    [SerializeField]
    protected PlaceableData m_placeableData;
    [SerializeField]
    protected TextMeshProUGUI m_text;
    public void SetData(PlaceableData data)
    {
        m_placeableData = data;
        m_text.text = data.name;
    }

    public void OnClick()
    {
        GameManager.Instance.OnBuildingBtnClicked(m_placeableData);
    }
}
