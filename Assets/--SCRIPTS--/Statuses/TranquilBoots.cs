using UnityEngine;

public class TranquilBoots : Status
{
    float steps = 0f;
    public override void Tick()
    {
        steps += Time.deltaTime;
        if (steps > 5f)
        {
            steps = 0f;
            target.Health += 2;
        }
    }

    public override void OnActivate()
    {
        target.MoveEvent.AddListener(Tick);
    }

    private void OnDisable()
    {
        target.MoveEvent.RemoveListener(Tick);
    }
}
