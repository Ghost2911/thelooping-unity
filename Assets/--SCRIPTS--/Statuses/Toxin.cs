using UnityEngine;

public class Toxin : Status
{
    public override void Tick()
    {
        target.Damage(new HitInfo(statusData.damageType,statusData.damagePerTick, 0f, Vector3.zero, statusData.color));
        statusAnimator.SetTrigger("Tick");
    }
}