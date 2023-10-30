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

    public PlaceableData DrawCard(bool shuffleIfEmtpy = false)
    {
        PlaceableData returnValue = null;
        if (IsEmtpy())
        {
            if (shuffleIfEmtpy)
            {
                GameManager.Instance.ShuffleDiscardIntoDrawPile();
            }
            else
            {
                return returnValue;
            }
        }
        returnValue = m_placeablesData[0];
        m_placeablesData.RemoveAt(0);

        return returnValue;
    }

    public void AddCard(PlaceableData data)
    {
        m_placeablesData.Add(data);
    }

    public bool IsEmtpy()
    {
        return m_placeablesData.Count == 0;
    }

    public static void TransferCard(int cardIndex, PlaceablesPile fromPile, PlaceablesPile toPile)
    {
        if (fromPile.m_placeablesData.Count > cardIndex)
        {
            var card = fromPile.m_placeablesData[cardIndex];
            fromPile.m_placeablesData.RemoveAt(cardIndex);
            toPile.m_placeablesData.Add(card);
        }
    }

    public static void TransferAllCards(PlaceablesPile fromPile, PlaceablesPile toPile, bool shuffleAfter = false)
    {
        for (int i = fromPile.m_placeablesData.Count - 1; i >= 0; i--)
        {
            var card = fromPile.m_placeablesData[i];
            fromPile.m_placeablesData.RemoveAt(i);
            toPile.m_placeablesData.Add(card);
        }

        if (shuffleAfter)
        {
            Shuffle(toPile.m_placeablesData);
        }
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
