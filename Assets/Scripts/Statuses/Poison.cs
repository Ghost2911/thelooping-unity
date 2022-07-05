using UnityEngine;

public class Poison : Status
{
    const float slowMultiplier = 1.5f;
    void OnEnable()
    {
        targetAnimator.speed = targetAnimator.speed / slowMultiplier;
        target.speedMultiplier = target.speedMultiplier / slowMultiplier;
    }
    void OnDisable()
    {
        targetAnimator.speed = targetAnimator.speed * slowMultiplier;
        target.speedMultiplier = target.speedMultiplier * slowMultiplier;
    }
    public override void Tick() 
    {
        target.Damage(statusData.damagePerTick, 0f, Vector3.zero, statusData.color);
        statusAnimator.SetTrigger("Tick");
    }
}