using UnityEngine;

public class Oiled : Status
{
    public float slowMultiplier;

    public override void OnActivate()
    {
        slowMultiplier = target.speedMultiplier / 2f;
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
        statusAnimator.SetTrigger("Tick");
    }
}
