public class Chilled : Status
{
    const float animatorSlowValue = 0.7f;
    const int slowMultiplier = 2;
    public override void OnActivate()
    {
        targetAnimator.speed = targetAnimator.speed - animatorSlowValue;
        target.speedMultiplier = target.speedMultiplier / slowMultiplier;
    }
    void OnDisable()
    {
        targetAnimator.speed = targetAnimator.speed + animatorSlowValue;
        target.speedMultiplier = target.speedMultiplier * slowMultiplier;
    }
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }
}