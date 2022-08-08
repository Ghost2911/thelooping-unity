using System.Collections;
using UnityEngine;

public class ToxicArmor : Status
{
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }
    public override void OnActivate()
    {
        target.DamageTakeEvent.AddListener(ToxicBlademail);
    }

    void ToxicBlademail(EntityStats stats)
    {
        if (stats != null)
            stats.AddStatus(statusData.additiveStatus);
    }

    private void OnDisable()
    {
        target.DamageTakeEvent.RemoveListener(ToxicBlademail);
    }
}
