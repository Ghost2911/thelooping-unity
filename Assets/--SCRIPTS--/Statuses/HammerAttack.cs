using UnityEngine;

public class HammerAttack : Status
{
    public override void Tick() { }

    public override void OnActivate()
    {
        target.AttackEvent.AddListener(CreateTornado);
    }

    void CreateTornado()
    {
        GameObject bullet = Instantiate(Resources.Load("Explossion/StoneHammer")
                    as GameObject, transform.position + target.direction, Quaternion.identity);
        bullet.GetComponent<Explossion>().owner = transform;
    }

    private void OnDisable()
    {
        target.AttackEvent.RemoveListener(CreateTornado);
    }
}
