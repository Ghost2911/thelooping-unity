using UnityEngine;

[CreateAssetMenu(fileName = "New Item Object", menuName = "Items/CreateNewStats")]
public class ItemStats : ScriptableObject
{
    [Header("Main info")]
    public SlotType slotType;
    public Sprite icon;
    public Color paintColor = Color.black;
    [Header("Item stats")]
    public StatsType type;
    public int additiveValue;
    public StatusData status;
    [Header("Item cost")]
    public CollectableItem costItem;
    public int costPrice;
}
