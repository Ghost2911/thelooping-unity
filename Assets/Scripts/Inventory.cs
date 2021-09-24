using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;
    public TextMeshProUGUI textmeshStats;
    private Dictionary<StatsType, int> stats = new Dictionary<StatsType, int>();

    private void Awake()
    {
        foreach (StatsType attribute in Enum.GetValues(typeof(StatsType)))
            stats.Add(attribute, 0);
        StatsPresentorUpdate();
    }

    public void Equip(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.type == item.slotType)
            {
                slot.Equip(item);
                ChangeStats(item.stats, item.additiveValue);
                break;
            }
        }
    }

    private void ChangeStats(StatsType statType, int value)
    {
         stats[statType] += value;
         StatsPresentorUpdate();
    }

    public void StatsPresentorUpdate()
    {
        textmeshStats.text = String.Empty;
        foreach (KeyValuePair<StatsType, int> attribute in stats)
        {
            textmeshStats.text += String.Format("{0}\t{1}\n",attribute.Key,attribute.Value);
        }
    }
}
