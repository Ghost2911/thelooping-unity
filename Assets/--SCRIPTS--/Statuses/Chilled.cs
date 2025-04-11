public class Chilled : Status
{
    const float animatorSlowValue = 4f;
    private float slowMultiplier;
    public override void OnActivate()
    {
        slowMultiplier = target.speedMultiplier / 4f;
        targetAnimator.speed /= animatorSlowValue;
        target.speedMultiplier -= slowMultiplier;
    }
    void OnDisable()
    {
        targetAnimator.speed *= animatorSlowValue;
        target.speedMultiplier += slowMultiplier;
    }
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }
}