using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;
    public TextMeshProUGUI textmeshStats;
    public TextMeshProUGUI textmeshCollectable;
    public TextMeshProUGUI handlerName;
    public EntityStats entityStats;
    public Dictionary<CollectableType, int> collectableItems = new Dictionary<CollectableType, int>();
    public static Inventory instance;

    public TargetPresentor[] bossMarks;
    private int bossMarksCursor = 0;

    private void Awake()
    {
        instance = this;
    }

    public void SetBaseSettings(EntityStats characterStats)
    {
        entityStats = characterStats;
        foreach (CollectableType item in Enum.GetValues(typeof(CollectableType)))
            collectableItems.Add(item, 0);
        foreach (Slot slot in slots)
            slot.stats = entityStats;
        StatsPresentorUpdate();
        CollectableItemPresentorUpdate();
    }

    public void SetHandlerName(string characterName)
    {
        handlerName.text = characterName;
    }

    public void Equip(ItemStats itemStats)
    {
        foreach (Slot slot in slots)
        {
            if (slot.type == itemStats.slotType)
            {
                slot.Equip(itemStats);
                if (itemStats.paintColor != new Color(0,0,0,0))
                    GlobalSettings.instance.player.GetComponent<ColorChanger>().SwapColor(slot.type, itemStats.paintColor);
                break;
            }
        }
    }

    public ItemStats GetItemStats(SlotType slotType)
    {
        foreach (Slot slot in slots)
            if (slot.type == slotType)
                return slot.itemStats;
        return null;
    }

    public bool ChangeCollectableItem(CollectableType collectableType, int value)
    {
        if (collectableItems[collectableType] + value >= 0)
        {
            collectableItems[collectableType] += value;
            CollectableItemPresentorUpdate();
            return true;
        }
        return false;
    }

    public void ChangeStats(StatsType statType, int value)
    {
         FieldInfo stats = entityStats.GetType().GetField(statType.ToString());
         stats.SetValue(entityStats, (int)stats.GetValue(entityStats)+value);
         StatsPresentorUpdate();
    }

    private void StatsPresentorUpdate()
    {
        textmeshStats.text = String.Format("Attack\t{0}\nArmor\t{1}\nSpeed\t{2}\n", entityStats.attack*entityStats.attackMultiplier, entityStats.armor* entityStats.armorMultiplier, entityStats.speed* entityStats.speedMultiplier);
    }

    private void CollectableItemPresentorUpdate()
    {
        textmeshCollectable.text = string.Join("", collectableItems.Select(p => $"{p.Value.ToString().PadLeft(2, '0')}" +
                                                $"  <sprite name=\"icon_res_{p.Key}\">"));
    }

    public TargetPresentor GetNextTargetPresentor()
    {
        return bossMarks[bossMarksCursor++];
    }
}
