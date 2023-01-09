using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float cooldown = 5f;
    public int trapDamage = 5;
    public StatusData trapStatus; 
    public bool isHold = false;
    public List<GameObject> trapTargets;
    private Animator _anim;
    private bool isActive = false;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("enemy"))
        {
            trapTargets.Add(other.gameObject);
            if (!isActive)
            {
                StartCoroutine("Attack");
                isActive = true;
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("enemy"))
        {
            trapTargets.Remove(other.gameObject);
        }
    }

    protected virtual void TakeDamage()
    {
        foreach (GameObject target in trapTargets)
        {
            if (target != null)
            {
                EntityStats stats = target.GetComponent<EntityStats>();
                stats.Damage(trapDamage, 0f, Vector3.zero, Color.red);
                if (isHold)
                {
                    target.transform.position = transform.position;
                    stats.animator.SetBool("isRun", false);
                }
                if (trapStatus != null)
                    stats.AddStatus(trapStatus);
            }
            else
                trapTargets.Remove(target);
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (_anim != null)
                _anim.SetTrigger("Attack");
            else
                TakeDamage();
            yield return new WaitForSeconds(cooldown);
            if (trapTargets.Count == 0)
            {
                isActive = false;
                break;
            }
        }
    }
}
