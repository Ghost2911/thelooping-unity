public class Stun : Status
{
    public override void OnActivate()
    {
        target.isStunned = true;
    }
    void OnDisable()
    {
        target.isStunned = false;
    }
    public override void Tick() { }
}