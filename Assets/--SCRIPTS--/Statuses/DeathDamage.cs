using UnityEngine;

public class DeathDamage : Status
{
    public override void OnActivate(){}

    public override void Tick(){}

    void OnDisable()
    { 
        target.Damage(new HitInfo(statusData.damageType, statusData.damagePerTick, 0f, Vector3.zero, statusData.color)); 
    }
}
