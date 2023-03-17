public class KnockbackPower : Status
{
    const int knockback = 50;
    public override void Tick() {}

    public override void OnActivate()
    {
        target.attackKnockback += knockback;
    }

    private void OnDisable()
    {
        target.attackKnockback -= knockback;
    }
}
