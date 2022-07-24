using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IThrowable
{
    public int damage;
    public float speed;
    public float affectedArea;

    public Vector3 destination;
    private Transform owner;

    public void InitialSetup(Transform target, Transform owner)
    {
        destination = target.position;
        this.owner = owner;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (transform.parent.position != destination)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, destination, Time.deltaTime * speed);
            yield return null;
        }
        yield return 0;
    }

    public void Explode()
    {
        StopAllCoroutines();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.parent.position, affectedArea);
        foreach (Collider enemy in hitEnemies)
            enemy.GetComponent<IDamageable>()?.Damage(damage, 0f, Vector3.zero, Color.red);
        Destroy(transform.parent.gameObject,0.3f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, affectedArea);
    }
}
