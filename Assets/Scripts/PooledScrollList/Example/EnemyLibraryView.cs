using System;
using PooledScrollList.Data;
using PooledScrollList.View;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class EnemyLibraryData : PooledData
{
    public int Attack;
    public string Name;
    public int Armor;
    public int Health;
    public int Speed;
}

public class EnemyLibraryView : PooledView
{
    public TextMeshProUGUI tmAttack;
    public TextMeshProUGUI tmName;
    public Image barArmor;
    public Image barHealth;
    public Image barSpeed;

    public override void SetData(PooledData data)
    {
        base.SetData(data);
        
        var exampleData = (EnemyLibraryData)data;
        tmName.text = exampleData.Name;
        tmAttack.text = exampleData.Attack.ToString();
        barArmor.fillAmount = exampleData.Armor / 20f;
        barHealth.fillAmount = exampleData.Health / 20f;
        barSpeed.fillAmount = exampleData.Speed / 20f;
    }
}
