
public class FrozenArmor : Status
{
    public override void Tick() { }

    public override void OnActivate()
    {
        target.DamageTakeEvent.AddListener(ReturnDamage);
    }

    void ReturnDamage(EntityStats stats)
    {
        if (stats != null)
            stats.AddStatus(statusData.additiveStatus);
    }

    private void OnDisable()
    {
        target.DamageTakeEvent.RemoveListener(ReturnDamage);
    }
}
