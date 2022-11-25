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

    [HideInInspector] public FloatingJoystick joystick;

    private CharacterController _characterController;
    private float attackTime = 0.3f;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        stats = GetComponent<EntityStats>();
        inventory = Inventory.instance;
        inventory.SetBaseSettings(stats);
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

        if (Input.GetKey(KeyCode.Delete))
            stats.Damage(999,0,Vector3.zero,Color.cyan);
    }

    private void Move(Vector3 movement)
    {
        stats.animator.SetBool("isRun", movement != Vector3.zero);

        if (movement != Vector3.zero)
        {
            if (stats.animator.GetCurrentAnimatorStateInfo(0).IsName("Flip"))
                return;
            stats.direction = movement.normalized;
            stats.MoveEvent.Invoke();
            _characterController.SimpleMove(stats.direction * stats.speed * stats.speedMultiplier);
            if (movement.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (movement.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Use()
    {
        if (!stats.isStunned)
        {
            if (stats.usableItem != null)
                stats.usableItem.Use(stats);
            else
            {
                Collider[] interactiveItems = Physics.OverlapSphere(transform.position, 2f);
                IUsable usableItem;
                foreach (Collider item in interactiveItems)
                {
                    usableItem = item.GetComponent<IUsable>();
                    if (usableItem != null)
                    {
                        usableItem.Use(stats);
                        Statistic.instance.OnItemUse(item.name);
                        break;
                    }
                }
            }
        }
    }

    private void Attack()
    {
        stats.AttackEvent.Invoke();
        Camera.main.GetComponent<CameraFollow>().CameraShake();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + stats.direction*1.5f, radiusAttack);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.transform != transform)
            {
                IDamageable damagable = enemy.GetComponent<IDamageable>();
                IStatusable statusable = enemy.GetComponent<IStatusable>();

                damagable?.Damage((int)(stats.attack * stats.attackMultiplier), stats.attackKnockback, stats.direction, Color.red, stats);
                StatusData weaponStatus = inventory.GetItemStats(SlotType.Weapons)?.status;
                if (weaponStatus != null)
                    if (!weaponStatus.useOnSelf)
                        statusable?.AddStatus(weaponStatus);
            }
        }
    }

    private void AttackRange()
    {
        GameObject bullet = Instantiate(rangePrefab, transform.position + stats.direction / 2, Quaternion.identity);
        bullet.GetComponent<IThrowable>().InitialSetup(transform.position+stats.direction.normalized * 20f,transform);
    }

    IEnumerator Flip()
    {
        Vector3 dir = stats.direction*1.1f;
        float time = 0.3f;
        stats.ignoreDamage = true;
        while (time > 0)
        {
            _characterController.SimpleMove(dir * stats.speed * stats.speedMultiplier);
            time -= Time.deltaTime;
            yield return null;
        }
        stats.ignoreDamage = false;
    }

    private void CreateDeadBody()
    {
        stats.DeathEvent.Invoke();
        GlobalSettings.instance.CreateCharacter();
        Destroy(gameObject);
    }

    public void AttackButtonClick() { if (!stats.isStunned) stats.animator.SetTrigger("Attack"); attackTime = 0.4f; }
    public void FlipButtonClick() { if (!stats.isStunned) if (!stats.animator.GetCurrentAnimatorStateInfo(0).IsName("Flip")) { stats.animator.SetTrigger("Flip"); StartCoroutine(Flip()); Debug.Log(gameObject.name); } }
    public void UseButtonClick() { if (!stats.isStunned) Use(); }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + stats.direction*1.5f, radiusAttack);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("collectable"))
        {
            inventory.ChangeCollectableItem(other.GetComponent<CollectableItem>().type, 1);
            Destroy(other.gameObject);
        }
    }
}