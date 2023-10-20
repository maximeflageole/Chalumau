using UnityEngine;

[CreateAssetMenu(fileName = "Supply", menuName = "New Supply")]
public class SupplyData: ScriptableObject
{
    public ESupplyType Type;
    public Sprite Sprite;
}