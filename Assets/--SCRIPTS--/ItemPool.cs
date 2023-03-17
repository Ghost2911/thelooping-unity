using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemPool
{
    public List<ItemStats> WeaponsPool;
    public List<ItemStats> BodyPool;
    public List<ItemStats> HelmetPool;
    public List<ItemStats> ArtifactPool;
    public List<ItemStats> BootsPool;
    public List<ItemStats> PotionPool;

    public ItemPool()
    {
        WeaponsPool = Resources.LoadAll<ItemStats>("Items/Weapons").ToList();
        BodyPool = Resources.LoadAll<ItemStats>("Items/Body").ToList();
        HelmetPool = Resources.LoadAll<ItemStats>("Items/Helmet").ToList();
        ArtifactPool = Resources.LoadAll<ItemStats>("Items/Artifact").ToList();
        BootsPool = Resources.LoadAll<ItemStats>("Items/Boots").ToList();
        PotionPool = Resources.LoadAll<ItemStats>("Items/Potions").ToList();
    }

    public ItemStats GetItemFromCategory(SlotType category)
    {
        List<ItemStats> items = GetCategory(category);
        if (items.Count != 0)
        {
            ItemStats resultItem = items[Random.Range(0,items.Count)];
            items.Remove(resultItem);
            return resultItem;
        }
        return null;
    }

    public List<ItemStats> GetCategory(SlotType slotType)
    {
        switch (slotType)
        {
            case SlotType.Weapons:
                return WeaponsPool;
            case SlotType.Body:
                return BodyPool;
            case SlotType.Helmet:
                return HelmetPool;
            case SlotType.Artifact:
                return ArtifactPool;
            case SlotType.Boots:
                return BootsPool;
            case SlotType.Potions:
                return PotionPool;
        }
        return null;
    }
}


