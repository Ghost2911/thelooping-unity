using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour, IDamageable, IStatusable
{
    public string entityName;
    public bool isDead = false;
    public bool isStunned = false;
    public bool ignoreDamage = false;
    public bool ignoreStatus = false;
    public float attackKnockback = 20f;

    public UnityEvent<int> HealthChangeEvent;
    public UnityEvent DeathEvent;
    public UnityEvent MoveEvent;
    public UnityEvent AttackEvent;
    public UnityEvent<StatusData> StatusTakeEvent;
    public UnityEvent<EntityStats> DamageTakeEvent;
    public UnityEvent<EntityStats> LethalDamageEvent;

    public int armor = 10;
    public int attack = 10;
    public DamageType damageType = DamageType.Impact;
    public float attackCooldown = 1f;
    public int speed = 10;
    public int regenValue = 1;   
    public float regenRefreshTime = 5f;

    public int[] resistance = {0,0,0,0};

    public int regenMultiplier = 1; 
    public float speedMultiplier = 1f;
    public float attackMultiplier = 1f;
    public float armorMultiplier = 1f;
    [HideInInspector]
    public Vector3 direction = Vector3.right;

    public int maxHealth = 12;
    private int health;
    public float knockbackMultiplier = 1f;
    public float statusDurationMultiplier = 1f;

    public IUsable usableItem;
    public Transform usableItemSlot;

    public Animator animator;
    public StatusData[] startStatuses;
    public List<Status> statusEffects;
    protected SpriteRenderer _render;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = !isDead;
        _render = GetComponent<SpriteRenderer>();
        health = maxHealth;
        StartCoroutine(HealthRegeneration());
        StartCoroutine(EvasionColor());

        foreach (StatusData status in startStatuses)
            AddStatus(status);
    }

    public void Damage(HitInfo hitInfo)
    {
        if (!ignoreDamage)
        {
            if (hitInfo.knockbackPower != 0)
                StartCoroutine(Knockback(hitInfo.direction, hitInfo.knockbackPower*knockbackMultiplier));
            if (hitInfo.damageValue != 0)
            {
                StartCoroutine(DamageColor(hitInfo.blindColor));
                int resultDamage = (hitInfo.ignoreArmor) ? hitInfo.damageValue : System.Convert.ToInt32(hitInfo.damageValue * (1 - armor / 20f));
                DamageTakeEvent?.Invoke(hitInfo.damageSource);
                if (Health - resultDamage <= 0)
                    LethalDamageEvent.Invoke(hitInfo.damageSource);
                Health -= resultDamage;
                if (isDead || hitInfo.damageSource != null)
                    Statistic.instance.OnDestroyObject(hitInfo.damageSource?.name,entityName);
            }
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
        direction = new Vector3(direction.x, 0f, direction.z);
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

    IEnumerator EvasionColor()
    {
        while (true)
        {
            _render.material.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddStatus(StatusData statusData)
    {
        if (statusData == null)
            return;
            
        if (!ignoreDamage)
        {
            if (!ignoreStatus)
            {
                System.Type statusType = System.Type.GetType(statusData.type.ToString());
                Status statusOnEntity = GetComponent(statusType) as Status;
                statusData.duration *= statusDurationMultiplier;
                if (statusOnEntity == null)
                {
                    Status status = gameObject.AddComponent(statusType) as Status;
                    status.Init(statusData);
                }
            }
            StatusTakeEvent.Invoke(statusData);
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
                    foreach (Status status in statusEffects)
                        Destroy(status);
                    animator.SetTrigger("Death");
                    DeathEvent.Invoke();
                    isStunned = true;
                    isDead = true;
                }
                if (health > maxHealth)
                    health = maxHealth;
                HealthChangeEvent.Invoke(health);
            }
        }
    }
}
