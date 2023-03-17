
public class Armored : Status
{
    public override void Tick() { }

    public override void OnActivate()
    {
        target.armorMultiplier *= 1.5f;
    }

    private void OnDisable()
    {
        target.armorMultiplier /= 1.5f;
    }
}
