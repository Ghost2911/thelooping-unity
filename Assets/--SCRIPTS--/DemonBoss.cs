using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss : Unit
{
    private Coroutine _cor = null;
    public GameObject splashAttack;
    public Transform _target;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        stats.HealthChangeEvent.AddListener(BossSlide);
    }

    public void BossSlide(int damage)
    {
        if (!stats.animator.GetBool("isRun"))
        {
            StopAllCoroutines();
            _cor = StartCoroutine(Move());
        }
    }
    void Update()
    {
        if (_target != null)
        {
            if (_cor == null)
                _cor = StartCoroutine("Attack1" /*+ Random.Range(1, 4)*/);
        }
    }

    IEnumerator Move()
    {
        stats.animator.SetBool("isRun", true);
        yield return new WaitForSeconds(5f);
        stats.animator.SetBool("isRun", false);
        _cor = null;
    }
                
    IEnumerator Attack1()
    {
        stats.animator.SetTrigger("Attack_1");
        yield return new WaitForSeconds(2f);
        _cor = null;
    }

    IEnumerator Attack2()
    {
        if (_target != null)
        {
            
            yield return new WaitForSeconds(6f);
        }
        _cor = null;
    }
    IEnumerator Attack3()
    {
        stats.animator.SetTrigger("Attack_2");
        yield return new WaitForSeconds(1f);
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
