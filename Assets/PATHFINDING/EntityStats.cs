using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour, IDamageable
{
    public string entityName;
    public bool isDead = false;
    public bool isStunned = false;
    public float speed = 20;
    public float attackRange = 0.25f;
    public UnityEvent<int> HealthChangeEvent;
    public UnityEvent EntityDeathEvent;

    public int baseArmor = 10;
    public int baseDamage = 10;
    public int baseSpeed = 10;
    public Dictionary<StatsType, int> additiveStats = new Dictionary<StatsType, int>();

    public int maxHealth = 12;
    public int health;

    private SpriteRenderer _render;
    public Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    public void Damage(int damage)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Flip"))
        {
            DamageColor();
            int resultDamage = Mathf.Clamp(damage, 0, 10000);
            Health -= resultDamage;
        }
    }

    IEnumerator DamageColor()
    {
        _render.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _render.material.color = Color.white;
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (!isDead)
            {
                health = value;
                StartCoroutine("DamageColor");
                if (health <= 0)
                {
                    health = 0;
                    animator.SetTrigger("Death");
                    isDead = true;
                }
                if (health > maxHealth)
                    health = maxHealth;
                HealthChangeEvent.Invoke(health);
            }
        }
    }

}
