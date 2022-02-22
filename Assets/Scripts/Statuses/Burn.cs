public class Burn : Status
{
    public override void OnStatusEnable() {}
    public override void OnStatusDisable() {}
    public override void Tick()
    {
        target.Damage(statusData.damagePerTick, statusData.color);
        statusAnimator.SetTrigger("Tick");
    }
}
