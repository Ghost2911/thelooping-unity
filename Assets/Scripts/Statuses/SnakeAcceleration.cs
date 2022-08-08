public class SnakeAcceleration : Status
{
    float animatorSlowValue = 0.7f;
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }
    public override void OnActivate()
    {
        targetAnimator.speed = targetAnimator.speed - animatorSlowValue;
    }

    private void OnDisable()
    {
        targetAnimator.speed = targetAnimator.speed + animatorSlowValue;
    }
}
