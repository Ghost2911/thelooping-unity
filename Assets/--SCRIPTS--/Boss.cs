using System.Collections;
using UnityEngine;

public class Boss: MonoBehaviour
{
    public Transform[] movePoints;
    public Turret[] turrets;
	public EntityStats target;
    private Vector3 targetPosition = Vector3.zero;
    public int health = 50;
    public bool isDead = false;
    public GameObject projectilePrefab;
    public GameObject splashAttack;
    public Transform _target;
    public string enemyTag;
    private SpriteRenderer _renderer;
    private Animator _animator;

    [HideInInspector]
	public EntityStats stats;
    private Vector3 startPosition;

	void Awake()
	{
		startPosition = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
		stats = GetComponent<EntityStats>();
		stats.speedMultiplier = Random.Range(stats.speedMultiplier - 0.4f, stats.speedMultiplier);
	}

    public void SetTarget(EntityStats targetStats)
	{
        _animator = GetComponent<Animator>();
		enemyTag = targetStats.tag;
		this.target = targetStats;
		targetPosition = targetStats.transform.position;
		StopAllCoroutines();
		StartCoroutine(Move());
	}

    IEnumerator Move()
    {
        int moveCount = 3;
        while (moveCount-- != 0)
        {
            _animator.SetBool("isRun", true);
            Vector3 newPosition = movePoints[Random.Range(0, movePoints.Length)].position;

            while (Vector3.Distance(newPosition, transform.position) > 0.1f)
            {
                SpriteFlip(newPosition-transform.position);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * stats.speed);
                yield return null;
            }
            _animator.SetBool("isRun", false);
        }
        SpriteFlip(_target.position-transform.position);
        yield return new WaitForSeconds(2f);
        StartCoroutine("Attack" + Random.Range(1, 4));
    }

    IEnumerator Attack1()
    {
        splashAttack.SetActive(true);
        yield return new WaitForSeconds(4f);
        splashAttack.SetActive(false);
        StartCoroutine(Move());
    }

    IEnumerator Attack2()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_target == null)
                break;
            GameObject bullet = Instantiate(projectilePrefab, transform.position, new Quaternion(0, 0, 0, 0));
            bullet.transform.LookAt(_target.position, Vector3.up);
            bullet.transform.Rotate(new Vector3(90, -90, 0)); 
            bullet.GetComponent<Projectile>().speed = 25;
            yield return new WaitForSeconds(0.5f);
           
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(Move());
    }
    IEnumerator Attack3()
    {
        foreach (Turret tur in turrets)
            if (!tur.enabled)
            {
                tur.enabled = true;
                yield return null;
                break;
            }
        StartCoroutine(Move());
    }
    
    private void SpriteFlip(Vector3 movement)
	{
		if (movement.x < 0)
			_renderer.flipX = false;
		else if (movement.x > 0)
			_renderer.flipX = true;
	}
}
