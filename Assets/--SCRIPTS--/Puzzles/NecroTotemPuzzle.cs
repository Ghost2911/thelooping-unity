using System.Collections.Generic;
using UnityEngine;

public class NecroTotemPuzzle : TotemPuzzle
{
    public int killCount = 6;
    public List<EntityStats> entityInRange = new List<EntityStats>();
    public Liquid liquidForBlood;
    public StatusData bloodStatus;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy") || other.CompareTag("Player"))
        {
            EntityStats stats = other.GetComponent<EntityStats>();
            if (stats != null)
            {
                stats.DeathEvent.AddListener(IncreaseDeathCount);
                entityInRange.Add(stats);
            }
        }
    }

    public void IncreaseDeathCount()
    {
        if (--killCount <= 0)
        {
            liquidForBlood.AddStatus(bloodStatus);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy") || other.CompareTag("Player"))
        {
            EntityStats stats = other.GetComponent<EntityStats>();
            if (stats != null)
            {
                stats.DeathEvent.RemoveListener(IncreaseDeathCount);
                entityInRange.Remove(stats);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (EntityStats stats in entityInRange)
            stats.DeathEvent.RemoveListener(IncreaseDeathCount);
    }
}
