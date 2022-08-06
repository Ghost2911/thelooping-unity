using System.Collections;
using UnityEngine;

public class FireSpirits : Status
{
    float timer = 0;
    public override void Tick()
    {
        StartCoroutine(SpiritTimer());
    }
    public override void OnActivate()
    {
        target.DamageTakeEvent.AddListener(BurnAttacker);
    }

    void BurnAttacker(EntityStats stats)
    {
        if (stats != null && timer > 0)
            stats.AddStatus(statusData.additiveStatus);
    }

    private void OnDisable()
    {
        target.DamageTakeEvent.RemoveListener(BurnAttacker);
    }

    IEnumerator SpiritTimer()
    {
        target.DamageTakeEvent.AddListener(BurnAttacker);
        yield return new WaitForSeconds(3f);
        target.DamageTakeEvent.RemoveListener(BurnAttacker);
    }
}
