using UnityEngine;
using System.Collections;
public class Unit : MonoBehaviour
{
	const float minPathUpdateTime = 0.5f;

	[Header("Enemy settings")]
	public float[] attackRange = new float[3];
	public float affectedArea = 3f;
	public Vector2 dashUseRange = new Vector2(10,999);
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
	private Vector3 pathDestination;
	private bool pathRequestSearched = false;
	private float startSpriteDirection;
	public string enemyTag;

	private Vector3[] circlePoints;

	void Awake()
	{
		startPosition = transform.position;
		startSpriteDirection = transform.localScale.x;
		attackNumber = Random.Range(0, 3);
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
		float distance = 0f; 
		if (!stats.isStunned && !isAttacking)
		{
			if (path.Length != 0)
			{
				Vector3 currentWaypoint = path[0];
				stats.animator.SetBool("isRun", true);

				while (!stats.isStunned)
				{
					distance = Vector3.Distance(transform.position, target.transform.position);

					//если достиг текущей точки обхода (тут можно проверять атаки на ренж)
					if (transform.position == currentWaypoint)
					{
						targetIndex++;


						if (dashUseRange.x < distance && dashUseRange.y > distance)
						{	
							stats.animator.SetBool("isRun", false);
							isAttacking = true; //чтобы не запускало поиск пути
							StartCoroutine("Charging"); 
							yield break;
						}

						//достиг конца пути
						if (targetIndex >= path.Length)
						{
							stats.animator.SetBool("isRun", false);
							if (target != null)
							{
								isAttacking = true;
								StartCoroutine("Attacking");
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
					distance = Vector3.Distance(transform.position, target.transform.position);
					StartCoroutine("Attacking");
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
						Debug.Log("PathUpdate");
						pathRequestSearched = true;

						pathDestination = GetPathDestination(attackNumber);
						PathRequestManager.RequestPath(GetInstanceID(), transform.position, pathDestination, OnPathFound);
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
		isAttacking = false;
	}


	private Vector3 GetPathDestination(int attackNumber)
	{
		int pointCount = 5; 
		float radius = attackRange[attackNumber];
		circlePoints = new Vector3[pointCount];

        Vector3 dirToTarget = (transform.position - target.transform.position).normalized;
        float startAngle = Mathf.Atan2(dirToTarget.z, dirToTarget.x);

        float angleRange = Mathf.PI / 2f; 

        for (int i = 0; i < pointCount; i++)
        {
            float t = (float)i / (pointCount - 1) - 0.5f;
            float angle = startAngle + t * angleRange;

            float x = target.transform.position.x + radius * Mathf.Cos(angle);
            float z = target.transform.position.z + radius * Mathf.Sin(angle);
            Vector3 point = new Vector3(x, target.transform.position.y, z);

            circlePoints[i] = point;
		}
		return circlePoints[Random.Range(0, circlePoints.Length)];
	}

	IEnumerator Charging()
	{
		Vector3 startPosition = transform.position;
		Vector3 directionVector = target.transform.position - transform.position;
		Vector3 endPosition = transform.position + directionVector.normalized*dashRange;
		SpriteFlip(transform.position - target.transform.position);
		stats.animator.SetTrigger($"Charge");

		while (Vector3.Distance(transform.position, endPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition,stats.speed * Time.deltaTime);
            yield return null; 
        }
		stats.animator.SetTrigger($"Idle");
		yield return new WaitForSeconds(0.2f);
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

		if (circlePoints == null)
            return;

        Gizmos.color = Color.cyan;
        foreach (Vector3 point in circlePoints)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
	}
}
