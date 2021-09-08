using UnityEngine;
using System.Collections;
public class Unit : MonoBehaviour, IDamageable
{
	const float minPathUpdateTime = .2f;
	const float pathUpdateMoveThreshold = .1f;

	public Transform target;
	public float speed = 20;
	public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 10;

	SpriteRenderer _sprite;
	Animator _animator;
	public float attackRange = 1f;
	public float affectedArea = 3f;
	bool isDead = false;
	public bool followingPath;
	public static float attackCooldown = 2f;
	Vector3 direction;

	public GameObject[] drops;

	Vector3[] path;
	int targetIndex;

	public int health = 10;
	public int Health
	{
		get
		{
			return health;
		}
		set
		{
			if (health > 0)
			{
				health = value;
				StartCoroutine("DamageColor");
				if (health <= 0)
				{
					isDead = true;
					StopAllCoroutines();
					_animator.SetTrigger("isDead");
				}
			}
		}
	}

	void Start()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();
		target = PlayerInput.instance.transform;
		StartCoroutine("UpdatePath");
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
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
		{
			yield return new WaitForSeconds(1f);
		}
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target.position;

		while (true)
		{
			yield return new WaitForSeconds(minPathUpdateTime);

			if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
			{
				PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
				targetPosOld = target.position;
			}
		}
	}

	IEnumerator FollowPath()
	{
		if (!isDead)
		{
			StopCoroutine("Attacking");
			_animator.SetBool("isRun", true);
			Vector3 currentWaypoint = path[0];
			while (true)
			{
				if (transform.position == currentWaypoint)
				{
					targetIndex++;
					if (targetIndex >= path.Length)
					{
						_animator.SetBool("isRun", false);
						StartCoroutine("Attacking");
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}

				direction = (transform.position - currentWaypoint).normalized;
				SpriteFlip(direction);
				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
				yield return null;
			}
		}
	}

	IEnumerator Attacking()
	{
		while (true)
		{
			_animator.SetTrigger("isAttack");
			yield return new WaitForSeconds(attackCooldown);
		}
	}

	private void SpriteFlip(Vector3 movement)
	{
		if (movement.x < 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
		else if (movement.x > 0)
			transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private void Attack()
	{
		Collider[] hitEnemies = Physics.OverlapSphere(transform.position + (target.position - transform.position).normalized * attackRange, affectedArea);
		foreach (Collider enemy in hitEnemies)
			if (enemy.tag == "Player" && enemy.transform.root != transform)
				enemy.GetComponent<IDamageable>().Damage(5);
	}

	private void RangeAttack()
	{
		GameObject bullet = Instantiate(Resources.Load("bullet"), transform.position, Quaternion.LookRotation(transform.position, target.position)) as GameObject;
		bullet.transform.LookAt(target);
		bullet.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
		bullet.GetComponent<Projectile>().destination = target.position;
	}

	public void Damage(int damage)
	{
		Health -= damage;
	}

	private void CreateDeadBody()
	{
		foreach (GameObject drop in drops)
			Instantiate(drop,transform.position, new Quaternion(0f,0f,0f,0f));
		Destroy(this.gameObject);
		//need create sprite
	}

	IEnumerator DamageColor()
	{
		_sprite.color = new Color32(153, 0, 0, 255);
		yield return new WaitForSeconds(0.1f);
		_sprite.color = Color.white;
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
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}
