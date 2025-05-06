using UnityEngine;

public class AoeAttack : Status
{ 
    public override void Tick(){}

    public override void OnActivate()
    {
        target.AttackEvent.AddListener(CreateAOE);
    }

    void CreateAOE()
    {
        GameObject bullet = Instantiate(Resources.Load("Projectile/LiquidTimeToxin")
                    as GameObject, transform.position,Quaternion.Euler(90f, 0f, 0f));
    }

    private void OnDisable()
    {
        target.AttackEvent.RemoveListener(CreateAOE);
    }
}
