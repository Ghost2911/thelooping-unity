
public class Resistance : Status
{
    const float statusMultiplier = 0.5f;
    public override void OnActivate()
    {
        target.statusDurationMultiplier /= statusMultiplier;
    }
    void OnDisable()
    {
        target.statusDurationMultiplier *= statusMultiplier;
    }
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }
}
