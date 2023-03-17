using UnityEngine;

[CreateAssetMenu(fileName = "HouseUpgradeData", menuName = "HouseUpgrade (Create New)", order = 0)]
public class HouseUpgradeData : ScriptableObject
{
    public Color color;
    public Sprite icon;
    public string description;
    public CollectableItem costItem;
    public int costPrice;
}


