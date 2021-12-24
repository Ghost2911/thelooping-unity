using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour, IDamageable
{
    [Header("Base stats")]
    public string heroName;
    public float attackRange = 0.25f;
    public int maxHealth = 12;

    public int baseArmor = 10;
    public int baseDamage = 10;
    public int baseSpeed = 10;

    [Header("UI/Control")]
    public FloatingJoystick joystick;
    public GameObject revive;
    public Button btnAttack;
    public Button btnFlip;

    [Header("Other")]
    public bool isDead = false;
    public UnityEvent<int> HealthChangeEvent;
    public Inventory inventory;
    public GameObject rangePrefab;
    
    private CharacterController _characterController;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private Vector3 direction = new Vector3(0, 0, 0);

    public int health;
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
                    _animator.SetTrigger("Dead");
                    revive.SetActive(true);
                    isDead = true;
                    Destroy(GetComponent<CharacterController>());
                }
                if (health > maxHealth)
                    health = maxHealth;
                HealthChangeEvent.Invoke(health);
            }
        }
    }

    void Awake()
    {
        Dictionary<StatsType, int> baseStats = new Dictionary<StatsType, int>();
        baseStats.Add(StatsType.Armor,baseArmor);
        baseStats.Add(StatsType.Damage, baseDamage);
        baseStats.Add(StatsType.Speed, baseSpeed);
        inventory.baseStats = baseStats;
        rangePrefab.GetComponent<Projectile>().owner = transform;
    }
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        inventory.SetHandlerName(heroName);
        btnAttack.onClick.AddListener(delegate { if (!isDead) _animator.SetTrigger("Attack"); });
        btnFlip.onClick.AddListener(delegate { if (!isDead) { _animator.SetTrigger("Flip"); StartCoroutine(Flip()); } });
        Health = maxHealth;
    }

    void Update()
    {
        if (isDead)
            return;

        Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Move(movement);
    }

    private void Move(Vector3 movement)
    {
        _animator.SetBool("isRun", movement != Vector3.zero);

        if (movement != Vector3.zero)
        {       
            direction = movement.normalized;
            _characterController.SimpleMove(direction * (baseSpeed + inventory.stats[StatsType.Speed]/baseSpeed));
            if (movement.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (movement.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Attack()
    {
        Camera.main.GetComponent<CameraFollow>().CameraShake();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + direction / 4, attackRange);
        foreach (Collider enemy in hitEnemies)
            if (enemy.GetComponent<IDamageable>()!=null && enemy.transform.root != transform) 
                enemy.GetComponent<IDamageable>().Damage(inventory.stats[StatsType.Damage]);
    }

    private void AttackRange()
    {
        GameObject bullet = Instantiate(rangePrefab, transform.position + direction / 2, new Quaternion(0,0,0,0));
        bullet.transform.LookAt(transform.position + direction, Vector3.up);
        bullet.transform.Rotate(new Vector3(90, -90, 0));
    }

    public void Damage(int damage)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Flip"))
        {
            int resultDamage = Mathf.Clamp(damage - inventory.stats[StatsType.Armor], 0, 100);
            Health -= resultDamage;
        }
    }

    IEnumerator Flip()
    {
        Vector3 endPos = transform.position + direction*5f;
        float flipTime = 0f;
        while (flipTime < 0.6f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime*baseSpeed);
            flipTime += Time.deltaTime;
            yield return null;
        }
    }

    private void CreateDeadBody()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageColor()
    {
        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _renderer.material.color = Color.white;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, 0.1f) + direction / 4, attackRange);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "collectable")
            Destroy(other.gameObject);
    }
}
