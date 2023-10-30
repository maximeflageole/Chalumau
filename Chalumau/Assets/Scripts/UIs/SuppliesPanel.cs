using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuppliesPanel : MonoBehaviour
{
    [SerializeField]
    private List<ListOfImages> m_images = new List<ListOfImages>();

    public void SetSupplies(Dictionary<ESupplyType, int> currentSupplies)
    {
        foreach (var imageList in m_images)
        {
            foreach (var image in imageList.Images)
            {
                image.enabled = false;
            }
        }
        foreach (var supply in currentSupplies)
        {
            var sprite = SuppliesManager.Instance.SuppliesDictionary[supply.Key].Sprite;
            for (int j = 0; j < currentSupplies[supply.Key]; j++)
            {
                m_images[(int)supply.Key].Images[j].sprite = sprite;
                m_images[(int)supply.Key].Images[j].enabled = true;
            }
        }
    }
}

[System.Serializable]
public class ListOfImages
{
    public List<Image> Images = new List<Image>();
}
