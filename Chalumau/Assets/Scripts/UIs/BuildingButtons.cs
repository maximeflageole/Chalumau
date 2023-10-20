using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButtons : MonoBehaviour
{
    [SerializeField]
    protected PlaceableData m_placeableData;

    public void OnClick()
    {
        GameManager.Instance.OnBuildingBtnClicked(m_placeableData);
    }
}
