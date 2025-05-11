using System.Collections;
using UnityEngine;

public class MechaBoss: MonoBehaviour
{
    public MoonObelisk[] moonObelisk;
	public EntityStats target;
    private Vector3 targetPosition = Vector3.zero;
    public int health = 50;
    public bool isDead = false;
    public GameObject projectilePrefab;
    public GameObject splashAttack;
    public Transform _target;
    public string enemyTag;

    private int currentMoonObelisk;
    private SpriteRenderer _renderer;
    private Animator _animator;

    [HideInInspector]
	public EntityStats stats;
    private Vector3 startPosition;
    private int attackNumber = 3;

	void Awake()
	{
		startPosition = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
		stats = GetComponent<EntityStats>();
		stats.speedMultiplier = Random.Range(stats.speedMultiplier - 0.4f, stats.speedMultiplier);
	}

    public void SetTarget()
	{
        EntityStats targetStats = GlobalSettings.instance.GetCurrentPlayer();
        _animator = GetComponent<Animator>();
		enemyTag = targetStats.tag;
		this.target = targetStats;
		targetPosition = targetStats.transform.position;
		StopAllCoroutines();
		StartCoroutine(Attacking());	
	}

    public void ResetTarget()
	{
		this.target = null;
		StopAllCoroutines();
	}


    IEnumerator Attacking()
    {
        while (target!=null)
        {
            SpriteFlip(transform.position - target.transform.position);
            stats.animator.SetTrigger($"Attack{attackNumber + 1}");
            yield return null;
            AnimationClip clip = stats.animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            float length = clip.length;
            yield return new WaitForSeconds(length / stats.animator.speed + stats.attackCooldown);
            attackNumber = Random.Range(0, 3);
        }
    }

    public void Attack1()
    {
        Instantiate(splashAttack, transform.position,Quaternion.Euler(90f, 0f, 0f));
    }

    IEnumerator Attack2()
    {
        if (projectilePrefab != null)
		{
            Vector3 targetDirection = target.direction*5;
            Vector3 targetPosition = target.transform.position;
			GameObject throwable = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation) as GameObject;
			throwable.GetComponentInChildren<IThrowable>().InitialSetup(targetPosition  + new Vector3(targetDirection.x, 0, targetDirection.z), transform);
            yield return new WaitForSeconds(0.5f);

            throwable = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation) as GameObject;
			throwable.GetComponentInChildren<IThrowable>().InitialSetup(targetPosition  + new Vector3(-targetDirection.x, 0, -targetDirection.z), transform);
            yield return new WaitForSeconds(0.5f);
            
            throwable = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation) as GameObject;
			throwable.GetComponentInChildren<IThrowable>().InitialSetup(targetPosition  + new Vector3(-targetDirection.x, 0, targetDirection.z), transform);
            yield return new WaitForSeconds(0.5f);

            throwable = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation) as GameObject;
			throwable.GetComponentInChildren<IThrowable>().InitialSetup(targetPosition  + new Vector3(targetDirection.x, 0, -targetDirection.z), transform);
            yield return new WaitForSeconds(0.5f);

            throwable = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation) as GameObject;
			throwable.GetComponentInChildren<IThrowable>().InitialSetup(targetPosition, transform);
		}

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Attack3()
    {
        yield return new WaitForSeconds(0.1f);
    }
    
    private void SpriteFlip(Vector3 movement)
	{
		if (movement.x < 0)
			_renderer.flipX = false;
		else if (movement.x > 0)
			_renderer.flipX = true;
	}
}
