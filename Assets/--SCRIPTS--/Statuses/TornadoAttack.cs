using UnityEngine;

public class TornadoAttack : Status
{ 
    public override void Tick(){}

    public override void OnActivate()
    {
        target.AttackEvent.AddListener(CreateTornado);
    }

    void CreateTornado()
    {
        GameObject bullet = Instantiate(Resources.Load("Projectile/Tornado")
                    as GameObject, transform.position + target.direction / 2, Quaternion.identity);
        bullet.GetComponent<IThrowable>().InitialSetup(transform.position + target.direction.normalized * 20f, transform);
    }

    private void OnDisable()
    {
        target.AttackEvent.RemoveListener(CreateTornado);
    }
}
