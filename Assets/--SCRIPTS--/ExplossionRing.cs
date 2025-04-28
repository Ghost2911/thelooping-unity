using UnityEngine;

public class ExplossionRing : Explossion
{
    public float ringRange;

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(other.transform.position, transform.position) > ringRange)
        {
            if (other.transform != owner)
            {
                IDamageable damagable = other.GetComponent<IDamageable>();
                IStatusable statusable = other.GetComponent<IStatusable>();

                damagable?.Damage(new HitInfo(damageType, damage, knockbackForce, new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1)).normalized, Color.red));
                if (status != null) statusable?.AddStatus(status);
            }
        }
    }
    void OnDrawGizmos()
    {
        /*SphereCollider sphereCollider = GetComponent<SphereCollider>();

        float colliderRadius = sphereCollider.radius * transform.lossyScale.x;
        Vector3 center = transform.TransformPoint(sphereCollider.center);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, colliderRadius);
        Gizmos.DrawWireSphere(center, Mathf.Max(0, colliderRadius - ringRange));
        */
    }
}
