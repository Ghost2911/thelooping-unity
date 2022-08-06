using System.Collections;
using UnityEngine;

public class ProjectilePuncture : MonoBehaviour, IThrowable
{
    public int damage;
    public float speed = 10f;
    public StatusData status;

    public Transform owner; 
    private Vector3 target;

    public void InitialSetup(Vector3 target, Transform owner)
    {
        this.target = target;
        this.owner = owner;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
            yield return null;
        }
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != owner && !other.isTrigger)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().Damage(damage, 0f, Vector3.zero, Color.red);
                if (status != null)
                    other.GetComponent<IStatusable>()?.AddStatus(status);
            }
        }
    }
}
