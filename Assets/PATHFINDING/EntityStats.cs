using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour, IDamageable, IStatusable
{
    public string entityName;
    public bool isDead = false;
    public bool isStunned = false;
    public bool isInvulnerability = false;
    public float attackKnockback = 20f;

    [HideInInspector]
    public UnityEvent<int> HealthChangeEvent;
    [HideInInspector]
    public UnityEvent DeathEvent;
    [HideInInspector]
    public UnityEvent MoveEvent;
    [HideInInspector]
    public UnityEvent AttackEvent;
    [HideInInspector]
    public UnityEvent<EntityStats> DamageTakeEvent;

    public int armor = 10;
    public float armorMultiplier = 1f;
    public int attack = 10;
    public int attackMultiplier = 1;
    public int speed = 10;
    public float speedMultiplier = 1f;

    public int regenValue = 1;
    public int regenMultiplier = 1;
    public float regenRefreshTime = 5f;

    public int maxHealth = 12;
    private int health;

    public Animator animator;
    public List<Status> statusEffects;
    private SpriteRenderer _render;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        _render = GetComponent<SpriteRenderer>();
        health = maxHealth;
        StartCoroutine(HealthRegeneration());
    }

    public void Damage(int damage, float knockbackPower, Vector3 direction, Color blindColor, EntityStats damageSource = null)
    {
        if (!isInvulnerability)
        {
            if (knockbackPower != 0)
                StartCoroutine(Knockback(direction, knockbackPower));
            StartCoroutine(DamageColor(blindColor));
            DamageTakeEvent.Invoke(damageSource);
            int resultDamage = System.Convert.ToInt32(damage *(1-armor/20f));
            Health -= resultDamage;
        }
    }

    IEnumerator HealthRegeneration()
    {
        while (true)
        {
            Health += regenValue * regenMultiplier;
            yield return new WaitForSeconds(regenRefreshTime);
        }
    }

    IEnumerator Knockback(Vector3 direction, float power)
    {
        float timer = 0.2f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position += direction * power * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DamageColor(Color blindColor)
    {
        _render.material.color = blindColor;
        yield return new WaitForSeconds(0.1f);
        _render.material.color = Color.white;
    }

    public void AddStatus(StatusData statusData)
    {
        if (!isInvulnerability)
        {
            System.Type statusType = System.Type.GetType(statusData.type.ToString());
            Status statusOnEntity = GetComponent(statusType) as Status;

            if (statusOnEntity != null)
                statusOnEntity.duration = statusData.duration;
            else
            {
                Status status = gameObject.AddComponent(statusType) as Status;
                status.statusData = statusData;
                status.Init();
            }
        }
    }

    public void RemoveStatus(StatusData statusData)
    {
        if (statusData != null)
        {
            System.Type statusType = System.Type.GetType(statusData.type.ToString());
            Status statusOnEntity = GetComponent(statusType) as Status;

            if (statusOnEntity != null)
                Destroy(statusOnEntity);
        }
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
