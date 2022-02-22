public class Chilled : Status
{
    const int slowMultiplier = 2;
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
    public override void Tick(){}
}