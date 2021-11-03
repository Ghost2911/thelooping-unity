using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;
    public TextMeshProUGUI textmeshStats;
    public TextMeshProUGUI textmeshCollectable;
    public TextMeshProUGUI handlerName;
    public Dictionary<StatsType, int> stats = new Dictionary<StatsType, int>();
    public Dictionary<CollectableType, int> collectableItems = new Dictionary<CollectableType, int>();

    [HideInInspector]
    public Dictionary<StatsType, int> baseStats = new Dictionary<StatsType, int>();

    private void Awake()
    {
        foreach (StatsType attribute in Enum.GetValues(typeof(StatsType)))
            stats.Add(attribute, baseStats[attribute]);
        foreach (CollectableType item in Enum.GetValues(typeof(CollectableType)))
            collectableItems.Add(item, 0);
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
                break;
            }
        }
    }

    public void ChangeCollectableItem(CollectableType collectableType, int value)
    {
        collectableItems[collectableType] += value;
        CollectableItemPresentorUpdate();
    }

    public void ChangeStats(StatsType statType, int value)
    {
         stats[statType] += value;
         StatsPresentorUpdate();
    }

    private void StatsPresentorUpdate()
    {
        textmeshStats.text = String.Empty;
        foreach (KeyValuePair<StatsType, int> attribute in stats)
        {
            textmeshStats.text += String.Format("{0}\t{1}\n",attribute.Key,attribute.Value);
        }
    }

    private void CollectableItemPresentorUpdate()
    {
        textmeshCollectable.text = string.Join("", collectableItems.Select(p => $"{p.Value.ToString().PadLeft(2, '0')}  <sprite name=\"resources_basic_{Convert.ToInt32(p.Key)}\">"));
    }
}
