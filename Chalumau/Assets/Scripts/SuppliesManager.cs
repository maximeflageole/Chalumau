using System.Collections.Generic;
using UnityEngine;

public class SuppliesManager : MonoBehaviour
{
    public static SuppliesManager Instance;

    [field:SerializeField]
    private List<SupplyData> SuppliesData { get; set; } = new List<SupplyData>();
    public Dictionary<ESupplyType, SupplyData> SuppliesDictionary { get; private set; } = new Dictionary<ESupplyType, SupplyData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SuppliesDictionary.Clear();
        foreach (var data in SuppliesData)
        {
            SuppliesDictionary.Add(data.Type, data);
        }
    }
}