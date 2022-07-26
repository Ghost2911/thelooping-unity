public class Frozen : Status
{
    public override void OnActivate()
    {
        target.isStunned = true;
    }
    public void OnDisable()
    {
        target.isStunned = false;
    }
    public override void Tick() { }
}