public class Bleed : Status
{
    bool tickIsActive = false;
    private void BleedActivate() => tickIsActive = true;
    public override void OnStatusEnable() { target.MoveEvent.AddListener(BleedActivate); }
    public override void OnStatusDisable() { target.MoveEvent.RemoveListener(BleedActivate); }
    public override void Tick()
    {
        if (tickIsActive)
        {
            target.Damage(statusData.damagePerTick, statusData.color);
            statusAnimator.SetTrigger("Tick");
            tickIsActive = false;
        }
    }
}