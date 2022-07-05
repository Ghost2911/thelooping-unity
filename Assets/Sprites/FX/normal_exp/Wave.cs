using UnityEngine;

public class Wave : MonoBehaviour
{
    public int damage = 1;
    public StatusData status;    
    public Vector3 direction; 
    public int waveIterations = 5;
    private int bufWaveIteration = 0;
    public Transform owner;

    public void Init(Vector3 direction,Transform owner)
    {
        this.direction = direction;
        this.owner = owner;
        transform.position = owner.position + direction.normalized;
    }

    public void Move()
    {
        if (waveIterations == bufWaveIteration)
        {
            transform.position = owner.position + direction.normalized;
            bufWaveIteration = 0;
        }
        transform.position += direction;
        bufWaveIteration++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != owner)
        {
            IDamageable damagable = other.GetComponent<IDamageable>();
            IStatusable statusable = other.GetComponent<IStatusable>();
            damagable?.Damage(damage, 0, Vector3.zero, Color.red);
            statusable?.AddStatus(status);
        }
    }
}
