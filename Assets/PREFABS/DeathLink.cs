using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLink : RadiusLink
{
    public StatusData deathStatus;
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(relatedObjectTag) && !other.transform.Equals(holder))
        {
            foreach (Link l in links)
                if (l.endPosition == null)
                {
                    l.SetPosition(holder, other.transform);
                    other.GetComponent<EntityStats>().AddStatus(deathStatus);
                    StartCoroutine(Revive());
                    break;
                }
        }
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(deathStatus.duration/3);
        EntityStats stats = holder.GetComponent<PlayerInput>().stats;
        stats.animator.enabled = true;
        yield return new WaitForSeconds(deathStatus.duration/4);
        stats.isDead = false;
    }
}
