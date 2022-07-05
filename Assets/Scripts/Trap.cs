using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float cooldown = 5f;
    public int trapDamage = 5;
    private Animator _anim;
    public List<GameObject> trapTargets;
    private bool isActive = false;


    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trap - enter");
        trapTargets.Add(other.gameObject);
        if (!isActive)
        {
            StartCoroutine("Attack");
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        trapTargets.Remove(other.gameObject);
    }

    private void TakeDamage()
    {
        foreach (GameObject target in trapTargets)
        {
            if (target != null)
                target.GetComponent<IDamageable>().Damage(trapDamage, 0f, Vector3.zero, Color.red);
            else
                trapTargets.Remove(target);
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            _anim.SetTrigger("Attack");
            yield return new WaitForSeconds(cooldown);
            if (trapTargets.Count == 0)
            {
                isActive = false;
                break;
            }
        }
    }
}
