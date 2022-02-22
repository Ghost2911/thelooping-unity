public class Poison : Status
{
    const float slowMultiplier = 1.5f;
    public override void OnStatusEnable()
    {
        targetAnimator.speed = targetAnimator.speed / slowMultiplier;
        target.speedMultiplier = target.speedMultiplier / slowMultiplier;
    }
    public override void OnStatusDisable()
    {
        targetAnimator.speed = targetAnimator.speed * slowMultiplier;
        target.speedMultiplier = target.speedMultiplier * slowMultiplier;
    }
    public override void Tick() 
    {
        target.Damage(statusData.damagePerTick, statusData.color);
        statusAnimator.SetTrigger("Tick");
    }
}