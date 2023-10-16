using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButtons : MonoBehaviour
{
    [SerializeField]
    protected BuildingData m_buildingData;

    public void OnClick()
    {
        GameManager.Instance.OnBuildingBtnClicked(m_buildingData);
    }
}
