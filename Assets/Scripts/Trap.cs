using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trap : MonoBehaviour
{
    public float cooldown = 5f;
    
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
            target.GetComponent<IDamageable>().Damage(5);
        }
    }

    IEnumerator Attack()
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
