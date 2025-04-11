using UnityEngine;
using System.Collections;
public class Unit : MonoBehaviour
{
	const float minPathUpdateTime = 0.5f;

	[Header("Enemy settings")]
	public float[] attackRange = new float[3];
	public float affectedArea = 3f;
	public float dashMultiplier = 0.8f;
	public float dashRange = 10f;
	public bool followingPath;
	public GameObject[] drops;
	public GameObject projectileItem;
	public DamageType damageType = DamageType.Impact;
	public StatusData weaponStatus;

	[HideInInspector]
	public EntityStats stats;
	public EntityStats target;
	private Vector3 targetPosition = Vector3.zero;
	private Vector3 startPosition;
	private Vector3 targetPositionBeforeAttack;
	private int attackNumber = 0;

	private Vector3 direction;
	private Vector3[] path;
	private int targetIndex;
	private bool isAttacking = false;
	private Vector3 pathOffset;
	private bool pathRequestSearched = false;
	private float startSpriteDirection;
	public string enemyTag;

	void Awake()
	{
		startPosition = transform.position;
		startSpriteDirection = transform.localScale.x;
		attackNumber = Random.Range(0, 3);
		pathOffset = new Vector3((Random.Range(0, 2) * 2 - 1) * attackRange[attackNumber], 0f, Random.Range(-attackRange[attackNumber] / 2, attackRange[attackNumber] / 2));
		stats = GetComponent<EntityStats>();
		stats.speedMultiplier = Random.Range(stats.speedMultiplier - 0.4f, stats.speedMultiplier);
		if (target != null)
			StartCoroutine(UpdatePath());
	}

    public void SetTarget(EntityStats targetStats)
	{
		enemyTag = targetStats.tag;
		this.target = targetStats;
		targetPosition = targetStats.transform.position;
		StopCoroutine(UpdatePath());
		StartCoroutine(UpdatePath());
	}

	public void SetTarget(Vector3 target)
	{
		this.target = null;
		targetPosition = target;
		StopCoroutine(UpdatePath());
		StartCoroutine(UpdatePath());
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			pathRequestSearched = false;
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		if (!stats.isStunned && !isAttacking)
		{
			if (path.Length != 0)
			{
				Vector3 currentWaypoint = path[0];
				stats.animator.SetBool("isRun", true);

				while (!stats.isStunned)
				{
					if (Vector3.Distance(transform.position, targetPosition) > 50f)
					{
						target = null;
						transform.position = startPosition;
						stats.animator.SetBool("isRun", false);
						yield break;
					}

					if (transform.position == currentWaypoint)
					{
						targetIndex++;
						if (targetIndex >= path.Length)
						{
							stats.animator.SetBool("isRun", false);
							if (target != null)
							{
								isAttacking = true;
								if (Random.Range(0f, 1f) < 0.6f)
									StartCoroutine("Attacking");
								else
									StartCoroutine("Charging");
							}
							yield break;
						}
						currentWaypoint = path[targetIndex];
					}

					direction = (transform.position - currentWaypoint).normalized;
					SpriteFlip(direction);
					transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, stats.speed * stats.speedMultiplier * Time.deltaTime);
					stats.MoveEvent.Invoke();
					yield return null;
				}
			}

			if (!isAttacking && !stats.isStunned)
			{
				stats.animator.SetBool("isRun", false);
				if (target != null)
				{
					isAttacking = true;
					if (Random.Range(0f, 1f) < 0.6f)
						StartCoroutine("Attacking");
					else
						StartCoroutine("Charging");
				}
			}
		}
	}

	void RaycastCheck()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, targetPosition, out hit, attackRange[attackNumber]))
		{
			Debug.DrawRay(transform.position, targetPosition * hit.distance, Color.yellow);
			Debug.Log(hit.transform.tag);
		}
	}

	IEnumerator UpdatePath()
	{
		if (Time.timeSinceLevelLoad < 1f)
			yield return new WaitForSeconds(0.1f);

		PathRequestManager.RequestPath(GetInstanceID(), transform.position, targetPosition, OnPathFound);

		if (target != null)
		{
			while (true)
			{
				yield return new WaitForSeconds(minPathUpdateTime);
				if (!isAttacking)
				{
					if (!pathRequestSearched)
					{
						//алгоритм распределения пути
						//алгоритм распределения пути
						//алгоритм распределения пути
						pathRequestSearched = true;
						PathRequestManager.RequestPath(GetInstanceID(), transform.position, target.transform.position + pathOffset, OnPathFound);
					}
				}
			}
		}
	}

	IEnumerator Attacking()
	{
		targetPositionBeforeAttack = target.transform.position;
		SpriteFlip(transform.position - target.transform.position);
		stats.animator.SetTrigger($"Attack{attackNumber + 1}");
		yield return new WaitForSeconds(stats.animator.GetCurrentAnimatorStateInfo(0).length + stats.attackCooldown);
		attackNumber = Random.Range(0, 3);
		pathOffset = new Vector3((Random.Range(0, 2) * 2 - 1) * attackRange[attackNumber], 0f, Random.Range(-attackRange[attackNumber] / 2, attackRange[attackNumber] / 2));
		isAttacking = false;
	}

	IEnumerator Charging()
    {
		Vector3 startPosition = transform.position;
		Vector3 directionVector = target.transform.position - transform.position;
		Vector3 endPosition = transform.position + (directionVector * dashMultiplier);
		SpriteFlip(transform.position - target.transform.position);
		stats.animator.SetTrigger($"Charge");

        float journeyLength = Vector3.Distance(startPosition, endPosition);
        float startTime = Time.time;

        while (transform.position != endPosition)
        {
            float distanceCovered = (Time.time - startTime) * stats.dashSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            yield return null;
        }
		stats.animator.SetTrigger($"Idle");
		pathOffset = new Vector3((Random.Range(0, 2) * 2 - 1) * dashRange, 0f, Random.Range(-dashRange / 2, dashRange / 2));
		yield return new WaitForSeconds(stats.attackCooldown);
		isAttacking = false;
    }

	private void SpriteFlip(Vector3 movement)
	{
		if (movement.x < 0)
			transform.localScale = new Vector3(1f * startSpriteDirection, 1f, 1f);
		else if (movement.x > 0)
			transform.localScale = new Vector3(-1f * startSpriteDirection, 1f, 1f);
	}

	private void Attack()
	{
		stats.AttackEvent.Invoke();
		Collider[] hitEnemies = Physics.OverlapSphere(transform.position + (targetPositionBeforeAttack - transform.position).normalized * attackRange[attackNumber], affectedArea);
		foreach (Collider enemy in hitEnemies)
			if (enemy.tag == enemyTag && enemy.transform.root != transform)
			{
				EntityStats enemyStats = enemy.GetComponent<EntityStats>();
				enemyStats?.Damage(new HitInfo(damageType,(int)(stats.attack * stats.attackMultiplier), 0f, Vector3.zero, Color.red, stats));
				if (weaponStatus != null)
					enemyStats?.AddStatus(weaponStatus);
			}
	}

	private void RangeAttack()
	{
		if (projectileItem != null)
		{
			stats.AttackEvent.Invoke();
			SpriteFlip(transform.position - target.transform.position);
			GameObject throwable = Instantiate(projectileItem, transform.position, Quaternion.identity) as GameObject;
			throwable.GetComponentInChildren<IThrowable>().InitialSetup(target.transform.position, transform);
		}
	}

	private void CreateDeadBody()
	{
		StopAllCoroutines();
		foreach (GameObject drop in drops)
			Instantiate(drop, transform.position, new Quaternion(0f, 0f, 0f, 0f));
		Destroy(this.gameObject);
	}

	void OnDrawGizmosSelected()
	{
		if (targetPosition != Vector3.zero)
			Gizmos.DrawWireSphere(transform.position + (targetPositionBeforeAttack - transform.position).normalized * attackRange[attackNumber], affectedArea);
	}

	public void OnDrawGizmos()
	{
        Vector3 textPosition = transform.position + Vector3.up + Vector3.forward;
        /*UnityEditor.Handles.Label(textPosition, transform.name);*/

		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex)
					Gizmos.DrawLine(transform.position, path[i]);
				else
					Gizmos.DrawLine(path[i - 1], path[i]);
			}
		}
	}
}
