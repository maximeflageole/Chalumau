using System.Collections.Generic;
using UnityEngine;

public class PlaceablesPile : MonoBehaviour
{
    [SerializeField]
    protected List<PlaceableData> m_placeablesData = new List<PlaceableData>();

    public void Shuffle()
    {
        Shuffle(m_placeablesData);
    }

    public PlaceableData DrawCard()
    {
        PlaceableData returnValue = null;
        if (m_placeablesData.Count <= 0)
        {
            return returnValue;
        }
        returnValue = m_placeablesData[0];
        m_placeablesData.RemoveAt(0);

        return returnValue;
    }

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
