using UnityEngine;

public class Poison : Status
{
    public float slowMultiplier;

    public override void OnActivate()
    {
        slowMultiplier = target.speedMultiplier / 4f;
        targetAnimator.speed = targetAnimator.speed - slowMultiplier;
        target.speedMultiplier = target.speedMultiplier - slowMultiplier;
    }

    void OnDisable()
    {
        targetAnimator.speed = targetAnimator.speed + slowMultiplier;
        target.speedMultiplier = target.speedMultiplier + slowMultiplier;
    }

    public override void Tick() 
    {
        target.Damage(statusData.damagePerTick, 0f, Vector3.zero, statusData.color);
        statusAnimator.SetTrigger("Tick");
    }
}