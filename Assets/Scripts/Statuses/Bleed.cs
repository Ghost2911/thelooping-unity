using UnityEngine;

public class Bleed : Status
{
    bool tickIsActive = false;
    private void BleedActivate() => tickIsActive = true;
    public void OnEnable() { target.MoveEvent.AddListener(BleedActivate);}
    public void OnDisable() { target.MoveEvent.RemoveListener(BleedActivate); }
    public override void Tick()
    {
        if (tickIsActive)
        {
            target.Damage(statusData.damagePerTick, 0f, Vector3.zero, statusData.color);
            statusAnimator.SetTrigger("Tick");
            tickIsActive = false;
        }
    }
}