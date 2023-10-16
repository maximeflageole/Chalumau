using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BuildingData m_buildingData;

    public void OnClick()
    {
        Instantiate(m_buildingData.m_prefab);
    }
}