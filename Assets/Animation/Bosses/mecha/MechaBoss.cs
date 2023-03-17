using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaBoss : MonoBehaviour, IDamageable
{
    private Animator _animator;
    private Coroutine _cor = null;

    public int health = 60;
    public bool isDead = false;
    public GameObject projectilePrefab;
    public GameObject laser;
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
    public void Damage(HitInfo hitInfo)
    {
        Health -= hitInfo.damageValue;
        if (!_animator.GetBool("isRun"))
        {
            StopAllCoroutines();
            _cor = StartCoroutine(Move());
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_target != null)
        {
            if (_cor == null)
                _cor = StartCoroutine("Attack"+Random.Range(1, 4));
        }
    }

    IEnumerator Move()
    {
        laser.SetActive(false);
        projectilePrefab.SetActive(false);
        _animator.SetBool("isRun", true);
        yield return new WaitForSeconds(5f);
        _animator.SetBool("isRun", false);
        _cor = null;
    }

    IEnumerator Attack1()
    {
        _animator.SetTrigger("Attack_3");
        laser.SetActive(true);
        Transform endPos = laser.GetComponent<LaserLine>().endPos;
        float timer = 5f;
        while (timer > 0)
        {
            int randomize = Random.Range(0, 2);
            endPos.position = Vector3.MoveTowards(endPos.position, _target.position, Time.deltaTime*5);
            timer -= Time.deltaTime;
            yield return null; 
        }
        laser.SetActive(false);
        _cor = null;
    }

    IEnumerator Attack2()
    {
        if (_target != null)
        {
            projectilePrefab.GetComponent<MechaArm>()._target = _target;
            _animator.SetTrigger("Attack_1");
            yield return new WaitForSeconds(1f);
            projectilePrefab.SetActive(true);

            laser.SetActive(true);
            Transform endPos = laser.GetComponent<LaserLine>().endPos;
            float timer = 5f;
            while (timer > 0)
            {
                int randomize = Random.Range(0, 2);
                endPos.position = Vector3.MoveTowards(endPos.position, _target.position, Time.deltaTime * 5);
                timer -= Time.deltaTime;
                yield return null;
            }
            laser.SetActive(false);

            yield return new WaitForSeconds(6f);
        }
        _cor = null;
    }
    IEnumerator Attack3()
    {
        _animator.SetTrigger("Attack_2");
        yield return new WaitForSeconds(1f);
        splashAttack.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        splashAttack.SetActive(false);
        _cor = null;
    }
    private void SpriteFlip(Vector3 movement)
    {
        if (movement.x < 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (movement.x > 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}
