using UnityEngine;

public class RingAOE : MonoBehaviour
{
    public int damage;
    public float ringRange;

    private CapsuleCollider _collider;

    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(other.transform.position, transform.position) > ringRange)
        {
            if (other.GetComponent<IDamageable>() != null)
                other.GetComponent<IDamageable>().Damage(damage);
        }
    }
}
