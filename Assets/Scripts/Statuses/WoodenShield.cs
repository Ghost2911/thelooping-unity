using System.Collections;
using UnityEngine;

public class WoodenShield : Status
{
    public override void Tick() 
    {
        StartCoroutine(ShieldTimer());
    }

    IEnumerator ShieldTimer()
    {
        target.isInvulnerability = true;
        yield return new WaitForSeconds(3f);
        target.isInvulnerability = false;
    }
}
