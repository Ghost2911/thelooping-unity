using System.Collections;
using UnityEngine;

public class WoodenShield : Status
{
    bool shieldBonusActive = false;
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
    }

    private void OnDisable()
    { 
        if (shieldBonusActive) 
            target.armorMultiplier /= 1.5f;
        target.DamageTakeEvent.RemoveListener(RemoveShield);
    }

    IEnumerator ShieldTimer()
    {
        shieldBonusActive = true;
        target.armorMultiplier *= 1.5f;
        yield return new WaitForSeconds(3f);
        target.armorMultiplier /= 1.5f;
        shieldBonusActive = false;
    }
}
