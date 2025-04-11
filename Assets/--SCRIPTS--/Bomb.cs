using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IThrowable
{
    public int damage;
    public DamageType damageType = DamageType.Impact;
    public float speed;
    public float affectedArea;
    public StatusData status;
    public GameObject createdObject;
    public Vector3 startOffset;
    public Animator animator;

    public Vector3 target;
    private Transform owner;

    public void InitialSetup(Vector3 target, Transform owner)
    {
        this.target = target;
        this.owner = owner;
        transform.parent.position = owner.position + startOffset;
        animator = GetComponent<Animator>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (transform.parent.position != target)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, Time.deltaTime * speed);
            yield return null;
        }
        animator.SetTrigger("bombExplode");
        Explode();
        yield return 0;
    }

    public void Explode()
    {
        StopAllCoroutines();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.parent.position, affectedArea);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.transform!=owner)
            {
                enemy.GetComponent<IDamageable>()?.Damage(new HitInfo(damageType,damage, 0f, Vector3.zero, Color.red));
                if (status!=null)
                    enemy.GetComponent<IStatusable>()?.AddStatus(status);
            }
        }
        Destroy(transform.parent.gameObject,0.5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, affectedArea);
    }
}
