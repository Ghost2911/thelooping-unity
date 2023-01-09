using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public int trapDamage = 5;
    public float knockbackForce = 10f;
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("enemy"))
        {
            EntityStats stats = other.GetComponent<EntityStats>();
            stats.Damage(trapDamage, knockbackForce, other.transform.position-transform.position, Color.red);
        }
    }
}
