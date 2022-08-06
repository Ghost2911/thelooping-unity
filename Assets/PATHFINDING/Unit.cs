using UnityEngine;
using System.Collections;
public class Unit : MonoBehaviour
{
	const float minPathUpdateTime = 0.5f;
	
	[Header("Enemy settings")]
	public float[] attackRange = new float[3];
	public float affectedArea = 3f;
	public bool followingPath;
	public GameObject[] drops;
	public GameObject projectileItem;
	public StatusData weaponStatus;
	
    [HideInInspector]
	public EntityStats stats;
	private Transform target;
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

	void Awake()
	{
		startPosition = transform.position;
		startSpriteDirection = transform.localScale.x;
		attackNumber = Random.Range(0, 3);
		pathOffset = new Vector3((Random.Range(0,2)*2-1)*attackRange[attackNumber], 0f, Random.Range(-attackRange[attackNumber]/2, attackRange[attackNumber] / 2));
		stats = GetComponent<EntityStats>();
		stats.speedMultiplier = Random.Range(stats.speedMultiplier-0.4f, stats.speedMultiplier);
		if (target != null)
			StartCoroutine(UpdatePath());
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
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
		if (!stats.isStunned)
		{
			if (path.Length != 0)
			{
				Vector3 currentWaypoint = path[0];
				stats.animator.SetBool("isRun", true);

				while (!stats.isStunned)
				{
					if (Vector3.Distance(transform.position, target.position) > 50f)
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
							isAttacking = true;
							StartCoroutine("Attacking");
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
				isAttacking = true;
				StartCoroutine("Attacking");
			}
		}
	}

	void RaycastCheck()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, target.position, out hit, attackRange[attackNumber]))
		{
			Debug.DrawRay(transform.position, target.position * hit.distance, Color.yellow);
			Debug.Log(hit.transform.tag);
		}
	}

	IEnumerator UpdatePath()
	{
		if (Time.timeSinceLevelLoad < 1f)
			yield return new WaitForSeconds(0.1f);

		PathRequestManager.RequestPath(GetInstanceID(),transform.position, target.position, OnPathFound);
		Vector3 targetPosOld = target.position;

		while (true)
		{
			yield return new WaitForSeconds(minPathUpdateTime);
			if (isAttacking == false)
			{
				if (!pathRequestSearched)
				{
					pathRequestSearched = true;
					PathRequestManager.RequestPath(GetInstanceID(),transform.position, target.position + pathOffset, OnPathFound);
					targetPosOld = target.position;
				}
			}
		}
	}

	IEnumerator Attacking()
	{
		targetPositionBeforeAttack = target.position;
		SpriteFlip(transform.position - target.position);
		stats.animator.SetTrigger($"Attack{attackNumber+1}");
		yield return new WaitForSeconds(Random.Range(stats.attackCooldown, stats.attackCooldown+1f));
		attackNumber = Random.Range(0, 3);
		pathOffset = new Vector3((Random.Range(0, 2) * 2 - 1) * attackRange[attackNumber], 0f, Random.Range(-attackRange[attackNumber] / 2, attackRange[attackNumber] / 2));
		isAttacking = false;
	}

	private void SpriteFlip(Vector3 movement)
	{
		if (movement.x < 0)
			transform.localScale = new Vector3(1f*startSpriteDirection, 1f, 1f);
		else if (movement.x > 0)
			transform.localScale = new Vector3(-1f*startSpriteDirection, 1f, 1f);
	}

	private void Attack()
	{
		stats.AttackEvent.Invoke();
		Collider[] hitEnemies = Physics.OverlapSphere(transform.position + (targetPositionBeforeAttack - transform.position).normalized * attackRange[attackNumber], affectedArea);
		foreach (Collider enemy in hitEnemies)
			if (enemy.tag == "Player" && enemy.transform.root != transform)
			{
				EntityStats entity = enemy.GetComponent<EntityStats>();
				entity.Damage(stats.attack * stats.attackMultiplier, 0f, Vector3.zero, Color.red, stats);
				if (weaponStatus != null)
					entity.AddStatus(weaponStatus);
			}
	}
	
	private void RangeAttack()
	{
		if (projectileItem != null)
		{
			stats.AttackEvent.Invoke();
			SpriteFlip(transform.position - target.position);
			GameObject throwable = Instantiate(projectileItem, transform.position, Quaternion.identity) as GameObject;
			throwable.GetComponentInChildren<IThrowable>().InitialSetup(target.position, transform);
		}
	}

	private void CreateDeadBody()
	{
		stats.DeathEvent.Invoke();
		StopAllCoroutines();
		PathRequestManager.RemovePath(GetInstanceID());
		foreach (GameObject drop in drops)
			Instantiate(drop,transform.position, new Quaternion(0f,0f,0f,0f));
		Destroy(this.gameObject);
	}

	void OnDrawGizmosSelected()
	{
		if (target != null)
			Gizmos.DrawWireSphere(transform.position + (target.position - transform.position).normalized * attackRange[0], affectedArea);
	}

	public void OnDrawGizmos()
	{
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
