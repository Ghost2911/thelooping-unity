using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    [Header("Entity stats")]
    public EntityStats stats;
    public Inventory inventory;
    public GameObject rangePrefab;

    [Header("UI/Control")]
    public FloatingJoystick joystick;
    public GameObject revive;
    public Button btnAttack;
    public Button btnFlip;
  
    private CharacterController _characterController;
    private Vector3 direction = new Vector3(0, 0, 0);

    void Awake()
    {
        rangePrefab.GetComponent<Projectile>().owner = transform;
        stats = GetComponent<EntityStats>();
        Dictionary<StatsType, int> baseStats = new Dictionary<StatsType, int>();
        baseStats.Add(StatsType.Armor, stats.baseArmor);
        baseStats.Add(StatsType.Damage, stats.baseDamage);
        baseStats.Add(StatsType.Speed, stats.baseSpeed);
        inventory.baseStats = baseStats;
        inventory.entityStats = stats;
    }
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        inventory.SetHandlerName(stats.entityName);
        btnAttack.onClick.AddListener(delegate { if (!stats.isDead) stats.animator.SetTrigger("Attack"); });
        btnFlip.onClick.AddListener(delegate { if (!stats.isDead) { stats.animator.SetTrigger("Flip"); StartCoroutine(Flip()); } });
    }

    void Update()
    {
        if (stats.isDead || stats.isStunned)
            return;

        Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Move(movement);
    }

    private void Move(Vector3 movement)
    {
        stats.animator.SetBool("isRun", movement != Vector3.zero);

        if (movement != Vector3.zero)
        {       
            direction = movement.normalized;
            _characterController.SimpleMove(direction * (stats.baseSpeed + stats.additiveStats[StatsType.Speed]/stats.baseSpeed));
            if (movement.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (movement.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Attack()
    {
        Camera.main.GetComponent<CameraFollow>().CameraShake();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + direction, stats.attackRange);
        foreach (Collider enemy in hitEnemies)
            if (enemy.GetComponent<IDamageable>() != null && enemy.transform != transform)
                enemy.GetComponent<IDamageable>().Damage(stats.additiveStats[StatsType.Damage]);
    }

    private void AttackRange()
    {
        GameObject bullet = Instantiate(rangePrefab, transform.position + direction / 2, new Quaternion(0,0,0,0));
        bullet.transform.LookAt(transform.position + direction, Vector3.up);
        bullet.transform.Rotate(new Vector3(90, -90, 0));
    }

    IEnumerator Flip()
    {
        Vector3 endPos = transform.position + direction*5f;
        float flipTime = 0f;
        while (flipTime < 0.6f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime*stats.baseSpeed);
            flipTime += Time.deltaTime;
            yield return null;
        }
    }

    private void CreateDeadBody()
    {
        revive.SetActive(true);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + direction, stats.attackRange);
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
