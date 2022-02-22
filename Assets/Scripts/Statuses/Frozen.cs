public class Frozen : Status
{
    public override void OnStatusEnable()
    {
        target.isStunned = true;
    }
    public override void OnStatusDisable()
    {
        target.isStunned = false;
    }
    public override void Tick() { }
}