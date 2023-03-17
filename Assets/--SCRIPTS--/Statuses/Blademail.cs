using UnityEngine;
public class Blademail : Status
{
    public override void Tick() { }

    public override void OnActivate()
    {
        target.DamageTakeEvent.AddListener(ReturnDamage);
    }

    void ReturnDamage(EntityStats stats)
    {
        if (stats != null)
            stats.Damage(new HitInfo(statusData.damageType, 10, 0, Vector3.zero, Color.red));
    }    

    private void OnDisable()
    {
        target.DamageTakeEvent.RemoveListener(ReturnDamage);
    }
}

