using UnityEngine;

public class ThrowTrap : Trap
{
    public GameObject rangePrefab;
    public Vector3 attackDirection;
    protected override void TakeDamage()
    {
        GameObject bullet = Instantiate(rangePrefab, transform.position + attackDirection / 2, Quaternion.identity);
        bullet.GetComponent<IThrowable>().InitialSetup(transform.position + attackDirection.normalized * 20f, transform);
    }
}
