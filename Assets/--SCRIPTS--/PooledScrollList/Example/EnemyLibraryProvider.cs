using System.Collections.Generic;
using JetBrains.Annotations;
using PooledScrollList.Controller;
using PooledScrollList.Data;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLibraryProvider : PooledDataProvider
{
    public PooledScrollRectBase ScrollRectController;
    public EntityStats[] Entity;

    private void Awake()
    {
        Entity = Resources.LoadAll<EntityStats>("Mobs") as EntityStats[];
    }

    public override List<PooledData> GetData()
    {
        var data = new List<PooledData>(Entity.Length);

        for (var i = 0; i < Entity.Length; i++)
        {
            data.Add(new EnemyLibraryData
            {
                Attack = Entity[i].attack,
                Name = Entity[i].entityName,
                Armor = Entity[i].armor,
                Health = Entity[i].maxHealth,
                Speed = Entity[i].speed
            });
        }

        return data;
    }

    [UsedImplicitly]
    public void Apply()
    {
        ScrollRectController.Initialize(GetData());
    }
}