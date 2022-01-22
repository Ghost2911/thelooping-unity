public class FireStatus : Status
{
    public int damagePerTick = 3;
    
    public override void Tick()
    {
        target.Damage(damagePerTick);
    }
}
