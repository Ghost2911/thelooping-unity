public class SnakeAcceleration : Status
{
    float animatorSpeedValue = 0.7f;
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }
    public override void OnActivate()
    {
        targetAnimator.speed = targetAnimator.speed + animatorSpeedValue;
    }

    private void OnDisable()
    {
        targetAnimator.speed = targetAnimator.speed - animatorSpeedValue;
    }
}
