using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuppliesPanel : MonoBehaviour
{
    [SerializeField]
    private List<ListOfImages> m_images = new List<ListOfImages>();

    public void SetSupplies(List<SupplyQty> supplies)
    {
        for (int i = 0; i < supplies.Count; i++)
        {
            var sprite = SuppliesManager.Instance.SuppliesDictionary[supplies[i].SupplyType].Sprite;
            for (int j = 0; j < supplies[i].Qty; j++)
            {
                m_images[i].Images[j].sprite = sprite;
            }
        }
    }
}

[System.Serializable]
public class ListOfImages
{
    public List<Image> Images = new List<Image>();
}
