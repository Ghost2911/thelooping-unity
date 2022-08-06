using UnityEngine;

public class Explossion : MonoBehaviour
{
    public float knockbackForce = 0f;
    public int damage = 10;
    public StatusData status;
    public Transform owner;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Explossion collider trigger - " + other.gameObject);
        if (other.transform != owner)
        {
            IDamageable damagable = other.GetComponent<IDamageable>();
            IStatusable statusable = other.GetComponent<IStatusable>();

            damagable?.Damage(damage, knockbackForce, new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1)).normalized, Color.red);
            if (status!=null) statusable?.AddStatus(status);
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
