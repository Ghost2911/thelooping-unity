using System.Collections;
using UnityEngine;

public class ProjectilePuncture : MonoBehaviour, IThrowable
{
    public int damage;
    public DamageType damageType = DamageType.Slash;
    public float speed = 10f;
    public StatusData status;
    public Vector3 offset;
    public bool spriteFlip = false;
    public Transform owner; 
    
    private Vector3 target;

    public void InitialSetup(Vector3 target, Transform owner)
    {
        transform.position+=offset;
        SpriteFlip(owner.position-target);
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

	private void SpriteFlip(Vector3 movement)
	{
		if (movement.x < 0)
			transform.localScale = new Vector3(spriteFlip?-1f:1f, 1f, 1f);
		else if (movement.x > 0)
			transform.localScale = new Vector3(spriteFlip?1f:-1f, 1f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != owner && !other.isTrigger)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().Damage(new HitInfo(damageType,damage, 0f, Vector3.zero, Color.red));
                if (status != null)
                    other.GetComponent<IStatusable>()?.AddStatus(status);
            }
        }
    }
}
