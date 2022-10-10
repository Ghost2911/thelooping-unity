using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Unit[] units;
    private Turret[] turrets;
    private void Awake()
    {
        units = transform.GetComponentsInChildren<Unit>();
        turrets = transform.GetComponentsInChildren<Turret>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Transform target = other.transform;
            foreach (Unit unit in units)
                if (unit!=null)
                    unit.SetTarget(target);
            foreach (Turret turret in turrets)
                turret.SetTarget(target);
        }
    }
}
