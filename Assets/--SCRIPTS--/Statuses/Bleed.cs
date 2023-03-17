using UnityEngine;

public class Bleed : Status
{
    bool tickIsActive = false;
    private void BleedActivate() => tickIsActive = true;
    public override void OnActivate() { target.MoveEvent.AddListener(BleedActivate);}
    public void OnDisable() { target.MoveEvent.RemoveListener(BleedActivate); }
    public override void Tick()
    {
        if (tickIsActive)
        {
            target.Damage(new HitInfo(statusData.damageType, statusData.damagePerTick, 0f, Vector3.zero, statusData.color, null, true));
            statusAnimator.SetTrigger("Tick");
            tickIsActive = false;
        }
    }
}