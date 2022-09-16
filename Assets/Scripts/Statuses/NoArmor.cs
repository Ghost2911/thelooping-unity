using Unity.Mathematics;

public class NoArmor : Status
{
    public override void Tick()
    {
        target.armorMultiplier = math.clamp(--target.armorMultiplier, 0,30);
        statusAnimator.SetTrigger("Tick");
    }
}
