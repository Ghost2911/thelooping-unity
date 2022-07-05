using UnityEngine;
using System.Collections;
public class Unit : MonoBehaviour
{
	const float minPathUpdateTime = 0.5f;
	[Header("Enemy settings")]

	public float attackRange = 1f;
	public float affectedArea = 3f;
	public bool followingPath;
	public static float attackCooldown = 1f;
	public GameObject[] drops;
	public EntityStats stats;
	public GameObject projectileBullet;

	private Transform target;
	private Vector3 direction;
	private Vector3[] path;
	private int targetIndex;
	private bool isAttacking = false;
	private Vector3 pathOffset;
	private bool pathRequestSearched = false;

	void Awake()
	{
		pathOffset = new Vector3((attackRange+affectedArea/3) * Mathf.Cos(Random.Range(0, 360)), 0, (attackRange+affectedArea/3)  * Mathf.Sin(Random.Range(0, 360)));
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
		if (!stats.isDead && !stats.isStunned)
		{
			if (path.Length != 0)
			{
				Vector3 currentWaypoint = path[0];
				stats.animator.SetBool("isRun", true);
				while (true)
				{
					if (Vector3.Distance(transform.position, target.position) < (attackRange + affectedArea))
					{
						stats.animator.SetBool("isRun", false);
						isAttacking = true;
						StartCoroutine("Attacking");
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
		}
	}

	void RaycastCheck()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, target.position, out hit, attackRange * 2))
		{
			Debug.DrawRay(transform.position, target.position * hit.distance, Color.yellow);
			Debug.Log(hit.transform.tag);
		}
	}
	IEnumerator UpdatePath()
	{
		if (Time.timeSinceLevelLoad < 1f)
			yield return new WaitForSeconds(0.1f);

		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		Vector3 targetPosOld = target.position;

		while (true)
		{
			yield return new WaitForSeconds(minPathUpdateTime);
			if (isAttacking == false)
			{
				if (!pathRequestSearched)
				{
					pathRequestSearched = true;
					PathRequestManager.RequestPath(transform.position, target.position + pathOffset, OnPathFound);
					targetPosOld = target.position;
				}
			}
		}
	}

	IEnumerator Attacking()
	{
		stats.animator.SetTrigger("isAttack");
		yield return new WaitForSeconds(Random.Range(attackCooldown, attackCooldown+1f));
		isAttacking = false;
	}
	

	private void SpriteFlip(Vector3 movement)
	{
		if (movement.x < 0)
			transform.localScale = new Vector3(1f, 1f, 1f);
		else if (movement.x > 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
	}

	private void Attack()
	{
		stats.AttackEvent.Invoke();
		SpriteFlip(transform.position - target.position);
		Collider[] hitEnemies = Physics.OverlapSphere(transform.position + (target.position - transform.position).normalized * attackRange, affectedArea);
		foreach (Collider enemy in hitEnemies)
			if (enemy.tag == "Player" && enemy.transform.root != transform)
				enemy.GetComponent<IDamageable>().Damage(stats.attack * stats.attackMultiplier, 0f, Vector3.zero, Color.red, stats);
	}
	
	private void RangeAttack()
	{
		if (projectileBullet != null)
		{
			stats.AttackEvent.Invoke();
			SpriteFlip(transform.position - target.position);
			GameObject bullet = Instantiate(projectileBullet, transform.position, Quaternion.LookRotation(transform.position, target.position)) as GameObject;
			bullet.transform.LookAt(target);
			bullet.transform.Rotate(new Vector3(90f, 0, 90f), Space.Self);
			bullet.GetComponent<Projectile>().destination = target.position;
			bullet.GetComponent<Projectile>().owner = transform;
		}
	}


	private void CreateDeadBody()
	{
		stats.DeathEvent.Invoke();
		foreach (GameObject drop in drops)
			Instantiate(drop,transform.position, new Quaternion(0f,0f,0f,0f));
		Destroy(this.gameObject);
	}

	void OnDrawGizmosSelected()
	{
		if (target != null)
			Gizmos.DrawWireSphere(transform.position + (target.position - transform.position).normalized * attackRange, affectedArea);
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
