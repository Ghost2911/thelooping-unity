public class Stun : Status
{
    void OnEnable()
    {
        target.isStunned = true;
    }
    void OnDisable()
    {
        target.isStunned = false;
    }
    public override void Tick() { }
}