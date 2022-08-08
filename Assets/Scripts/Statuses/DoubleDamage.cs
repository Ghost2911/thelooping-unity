
public class DoubleDamage : Status
{
    public override void OnActivate()
    {
        target.attackMultiplier += 0.5f; 
    }
    void OnDisable()
    {
        target.attackMultiplier -= 0.5f;
    }
    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }
}
