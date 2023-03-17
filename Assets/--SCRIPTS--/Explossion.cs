using UnityEngine;

public class Explossion : MonoBehaviour
{
    public float knockbackForce = 0f;
    public int damage = 10;
    public DamageType damageType = DamageType.Impact;
    public StatusData status;
    public Transform owner;

    private void Start()
    {
        Destroy(gameObject,GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
    public void TakeDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1f);

        foreach (Collider other in hitEnemies)
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

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
