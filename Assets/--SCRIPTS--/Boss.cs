using System.Collections;
using UnityEngine;

public class Boss : Unit
{
    private Animator _animator;
    public Transform[] movePoints;
    public Turret[] turrets;
    public int health = 50;
    public bool isDead = false;
    public GameObject projectilePrefab;
    public GameObject splashAttack;
    
    public Transform _target;
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
    public void Damage(int damage, float knockbackPower, Vector3 direction, Color blindColor, EntityStats damageSource = null, bool ignoreArmor = false)
    {
        Health -= damage;
        if (!_animator.GetBool("isRun"))
            StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        int moveCount = 10;
        while (moveCount-- != 0)
        {
            _animator.SetBool("isRun", true);
            Vector3 newPosition = movePoints[Random.Range(0, movePoints.Length)].position;

            while (Vector3.Distance(newPosition, transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 15f);
                yield return null;
            }
            _animator.SetBool("isRun", false);
        }
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
            yield return new WaitForSeconds(0.5f);
            bullet.GetComponent<Projectile>().speed = 25;
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
}