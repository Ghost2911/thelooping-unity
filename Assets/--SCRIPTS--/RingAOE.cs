using UnityEngine;

public class RingAOE : MonoBehaviour
{
    public int damage;
    public DamageType damageType;
    public float ringRange;

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(other.transform.position, transform.position) > ringRange)
        {
            if (other.GetComponent<IDamageable>() != null)
                other.GetComponent<IDamageable>().Damage(new HitInfo(damageType, damage, 0f, Vector3.zero, Color.red));
        }
    }
}
