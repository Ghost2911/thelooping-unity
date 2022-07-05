public class Frozen : Status
{
    public void OnEnable()
    {
        target.isStunned = true;
    }
    public void OnDisable()
    {
        target.isStunned = false;
    }
    public override void Tick() { }
}