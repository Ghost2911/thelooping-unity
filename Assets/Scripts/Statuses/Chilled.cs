public class Chilled : Status
{
    const int slowMultiplier = 2;
    public override void OnActivate()
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
    }
}