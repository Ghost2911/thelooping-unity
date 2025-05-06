using System.Collections;
using UnityEngine;

public class Boss: MonoBehaviour
{
    public MoonObelisk[] moonObelisk;
	public EntityStats target;
    public CircularMovement skulls;
    private Vector3 targetPosition = Vector3.zero;
    public int health = 50;
    public bool isDead = false;
    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;
    public GameObject splashAttack;
    public Transform _target;
    public string enemyTag;

    private int currentMoonObelisk;
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

    public void SetTarget()
	{
        EntityStats targetStats = GlobalSettings.instance.GetCurrentPlayer();
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
        Vector3 newPosition = Vector3.zero; 

        while (moveCount-- != 0)
        {
            _animator.SetBool("isRun", true);
            currentMoonObelisk = Random.Range(0, moonObelisk.Length);

            if (moonObelisk[currentMoonObelisk]!=null)
                newPosition = moonObelisk[currentMoonObelisk].transform.position;

            while (Vector3.Distance(newPosition, transform.position) > 0.1f)
            {
                SpriteFlip(newPosition-transform.position);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * stats.speed);
                yield return null;
            }
            if (moonObelisk[currentMoonObelisk].GiveSkull())
                skulls.AddSkull();
            _animator.SetBool("isRun", false);
        }
        SpriteFlip(_target.position-transform.position);
        moonObelisk[currentMoonObelisk].GiveSkull();
        yield return new WaitForSeconds(2f);
        StartCoroutine("Attack" + Random.Range(1, 4));
    }

    IEnumerator Attack1()
    {
        skulls.RemoveSkull();
        if (_target == null)
            yield return null;
        
        
        Vector3 direction = target.transform.position - transform.position;

        Vector3 perpendicularLeft = new Vector3(-direction.z, direction.y, direction.x); 
        Vector3 perpendicularRight = new Vector3(direction.z, direction.y, -direction.x); 

        Vector3 pointLeft = transform.position + perpendicularLeft*0.5f;
        Vector3 pointRight = transform.position + perpendicularRight*0.5f;

        GameObject bullet = Instantiate(projectilePrefab, pointLeft, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position + direction, transform);

        bullet = Instantiate(projectilePrefab, pointRight, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position + direction, transform);

        bullet = Instantiate(projectilePrefab, transform.position, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position + direction, transform);

        yield return new WaitForSeconds(1f);
        StartCoroutine(Move());
    }

    IEnumerator Attack2()
    {
        skulls.RemoveSkull();
        if (_target == null)
            yield return null;
        
        
        Vector3 direction = target.transform.position - transform.position;

        Vector3 perpendicularLeft = new Vector3(-direction.z, direction.y, direction.x); 
        Vector3 perpendicularRight = new Vector3(direction.z, direction.y, -direction.x); 

        Vector3 pointLeft = transform.position + perpendicularLeft*0.5f;
        Vector3 pointRight = transform.position + perpendicularRight*0.5f;

        GameObject bullet = Instantiate(projectilePrefab, pointLeft, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position + direction + perpendicularLeft*0.8f, transform);

        bullet = Instantiate(projectilePrefab, pointRight, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position + direction + perpendicularRight*0.8f , transform);

        bullet = Instantiate(projectilePrefab, transform.position, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position + direction, transform);

        yield return new WaitForSeconds(1f);
        StartCoroutine(Move());
    }

    IEnumerator Attack3()
    {
        if (_target == null)
            yield return null;

        skulls.RemoveSkull();
        GameObject bullet = Instantiate(projectilePrefab2, transform.position + Vector3.up, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position, transform);
        yield return new WaitForSeconds(1f);

        bullet = Instantiate(projectilePrefab2, transform.position + Vector3.up, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position, transform);
        yield return new WaitForSeconds(1f);

        bullet = Instantiate(projectilePrefab2, transform.position + Vector3.up, new Quaternion(0, 0, 0, 0));
        bullet.GetComponent<ProjectilePuncture>().InitialSetup(target.transform.position, transform);
        yield return new WaitForSeconds(1f);

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
