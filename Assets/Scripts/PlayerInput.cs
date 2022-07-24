using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [Header("Entity stats")]
    public EntityStats stats; 
    public float radiusAttack;
    public Inventory inventory;
    public GameObject rangePrefab;
   

    [Header("UI/Control")]
    public FloatingJoystick joystick;
    public GameObject revive;
    public Button btnAttack;
    public Button btnFlip;

    private CharacterController _characterController;
    private Vector3 direction = new Vector3(1, 0, 0);
    private float attackTime = 0.3f;

    void Start()
    {
        inventory = Inventory.instance;
        btnAttack.onClick.AddListener(delegate { stats.animator.SetTrigger("Attack"); attackTime = 0.4f; });
        btnFlip.onClick.AddListener(delegate { if (!stats.animator.GetCurrentAnimatorStateInfo(0).IsName("Flip")) { stats.animator.SetTrigger("Flip"); StartCoroutine(Flip()); } });

        rangePrefab.GetComponent<Projectile>().owner = transform;
        stats = GetComponent<EntityStats>(); 
        
        inventory.SetBaseSettings(stats);
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (stats.isDead || stats.isStunned)
            return;

        Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if (attackTime > 0)
            attackTime -= Time.deltaTime;
        else
            Move(movement);
    }

    private void Move(Vector3 movement)
    {
        stats.animator.SetBool("isRun", movement != Vector3.zero);

        if (movement != Vector3.zero)
        {
            if (stats.animator.GetCurrentAnimatorStateInfo(0).IsName("Flip"))
                return;
            direction = movement.normalized;
            stats.MoveEvent.Invoke();
            _characterController.SimpleMove(direction * stats.speed * stats.speedMultiplier);
            if (movement.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (movement.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);

        }
    }
    //stats.animator.GetCurrentAnimatorStateInfo(0).IsName("Flip")

    private void Attack()
    {
        stats.AttackEvent.Invoke();
        Camera.main.GetComponent<CameraFollow>().CameraShake();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + direction, radiusAttack);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.transform != transform)
            {
                IDamageable damagable = enemy.GetComponent<IDamageable>();
                IStatusable statusable = enemy.GetComponent<IStatusable>();

                damagable?.Damage(stats.attack * stats.attackMultiplier, stats.attackKnockback, direction, Color.red, stats);
                StatusData weaponStatus = inventory.GetItemStats(SlotType.Weapons)?.status;
                if (weaponStatus != null)
                    statusable?.AddStatus(weaponStatus);
            }
        }
    }

    private void AttackRange()
    {
        stats.AttackEvent.Invoke();
        GameObject bullet = Instantiate(rangePrefab, transform.position + direction / 2, new Quaternion(0, 0, 0, 0));
        bullet.transform.LookAt(transform.position + direction, Vector3.up);
        bullet.transform.Rotate(new Vector3(90, -90, 0));
    }

    IEnumerator Flip()
    {
        Vector3 dir = direction*1.1f;
        float time = 0.3f;
        stats.isInvulnerability = true;
        while (time > 0)
        {
            _characterController.SimpleMove(dir * stats.speed * stats.speedMultiplier);
            time -= Time.deltaTime;
            yield return null;
        }
        stats.isInvulnerability = false;
    }

    private void CreateDeadBody()
    {
        stats.DeathEvent.Invoke();
        revive.SetActive(true);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + direction, radiusAttack);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "collectable")
        {
            inventory.ChangeCollectableItem(other.GetComponent<CollectableItem>().type, 1);
            Destroy(other.gameObject);
        }
    }
}