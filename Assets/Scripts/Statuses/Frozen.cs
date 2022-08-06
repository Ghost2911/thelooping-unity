using UnityEngine;
public class Frozen : Status
{
    public override void OnActivate()
    {
        target.isStunned = true;
        targetAnimator.StartPlayback();
    }
    public void OnDisable()
    {
        target.isStunned = false;
        targetAnimator.StopPlayback();
    }
    public override void Tick()
    {
        target.Damage(statusData.damagePerTick, 0f, Vector3.zero, statusData.color);
    }
}