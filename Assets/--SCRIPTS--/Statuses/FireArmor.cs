using System.Collections;
using UnityEngine;

public class FireArmor : Status
{
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
        StartCoroutine(SpiritTimer());
    }

    void BurnAttacker(EntityStats stats)
    {
        if (stats != null)
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
