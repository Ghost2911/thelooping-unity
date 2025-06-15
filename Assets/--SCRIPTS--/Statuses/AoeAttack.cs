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
        GameObject bullet = Instantiate(statusData.additiveObject, transform.position, statusData.additiveObject.transform.rotation);
    }

    private void OnDisable()
    {
        target.AttackEvent.RemoveListener(CreateAOE);
    }
}
