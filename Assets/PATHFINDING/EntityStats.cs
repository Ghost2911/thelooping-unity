using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour, IDamageable, IStatusable
{
    public string entityName;
    public bool isDead = false;
    public bool isStunned = false;
    public float attackRange = 0.25f;

    [HideInInspector]
    public UnityEvent<int> HealthChangeEvent;
    [HideInInspector]
    public UnityEvent DeathEvent;
    [HideInInspector]
    public UnityEvent MoveEvent;
    [HideInInspector]
    public UnityEvent AttackEvent;

    public int baseArmor = 10;
    public int baseDamage = 10;
    public int baseSpeed = 10;

    public float speedMultiplier = 1f;
    public float attackMultiplier = 1f;

    public Dictionary<StatsType, int> additiveStats = new Dictionary<StatsType, int>();

    public int maxHealth = 12;
    public int health;

    private SpriteRenderer _render;
    public Animator animator;
    public List<Status> statusEffects;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        _render = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    public void Damage(int damage, Color blindColor)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Flip"))
        {
            StartCoroutine(DamageColor(blindColor));
            int resultDamage = Mathf.Clamp(damage, 0, 10000);
            Health -= resultDamage;
        }
    }

    IEnumerator DamageColor(Color blindColor)
    {
        _render.material.color = blindColor;
        yield return new WaitForSeconds(0.1f);
        _render.material.color = Color.white;
    }

    public void AddStatus(StatusData status)
    {
        System.Type statusType = System.Type.GetType(status.type.ToString());
        //if (gameObject.GetComponent<Status>().GetType() != statusType)
        gameObject.AddComponent(statusType);
        gameObject.GetComponent<Status>().statusData = status;
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
