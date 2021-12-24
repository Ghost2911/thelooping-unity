using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss : Unit, IDamageable
{
    private Coroutine _cor = null;
    public GameObject splashAttack;
    public Transform _target;
    
    public new void Damage(int damage)
    {
        Health -= damage;
        if (!_animator.GetBool("isRun"))
        {
            StopAllCoroutines();
            _cor = StartCoroutine(Move());
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        this.target = target;
        if (this.target!=null)
            StartCoroutine("UpdatePath");
        else
            StopCoroutine("UpdatePath");
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
        _animator.SetBool("isRun", true);
        yield return new WaitForSeconds(5f);
        _animator.SetBool("isRun", false);
        _cor = null;
    }
                
    IEnumerator Attack1()
    {
        _animator.SetTrigger("Attack_1");
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
        _animator.SetTrigger("Attack_2");
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
