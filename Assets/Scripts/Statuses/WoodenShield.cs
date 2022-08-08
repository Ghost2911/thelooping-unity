using System.Collections;
using UnityEngine;

public class WoodenShield : Status
{
    public override void OnActivate()
    {
        target.DamageTakeEvent.AddListener(RemoveShield);
    }

    public override void Tick() 
    {
        statusAnimator.SetTrigger("Tick");
        StartCoroutine(ShieldTimer());
    }
    void RemoveShield(EntityStats stats)
    {
        statusAnimator.SetTrigger("StatusEnd");
        target.isInvulnerability = false;
    }

    private void OnDisable()
    {
        target.DamageTakeEvent.RemoveListener(RemoveShield);
    }

    IEnumerator ShieldTimer()
    {
        target.isInvulnerability = true;
        yield return new WaitForSeconds(3f);
        target.isInvulnerability = false;
    }
}
