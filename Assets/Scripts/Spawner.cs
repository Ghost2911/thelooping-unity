using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Unit[] units;
    private void Awake()
    {
        units = transform.GetComponentsInChildren<Unit>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Transform target = other.transform;
            foreach (Unit unit in units)
                if (unit!=null)
                    unit.SetTarget(target);
        }
    }
}
