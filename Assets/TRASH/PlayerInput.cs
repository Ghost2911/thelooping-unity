using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour, IDamageable
{
    CharacterController _characterController;
    SpriteRenderer _renderer;
    public Animator _animator;

    public FloatingJoystick joystick;
    public GameObject revive;
    public Button btnAttack;

    public float attackRange = 0.25f;
    public float characterSpeed = 0.5f;
    private Vector3 direction = new Vector3(0, 0, 0);
    public bool isDead = false;

    public UnityEvent<int> HealthChangeEvent;

    public static PlayerInput instance { get; private set; }

    public int maxHealth = 12;
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
                    _animator.SetTrigger("isDead");
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
        instance = this;
    }
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        btnAttack.onClick.AddListener(delegate { if (!isDead) _animator.SetTrigger("isAttack"); });
        Health = maxHealth;
    }

    void Update()
    {
        if (isDead)
            return;

        //Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Move(movement);
    }

    private void Move(Vector3 movement)
    {
        _animator.SetBool("isRun", movement != Vector3.zero);

        if (movement != Vector3.zero)
        {
            _characterController.SimpleMove(movement.normalized * characterSpeed);
            if (movement.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (movement.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
            direction = movement.normalized;
        }
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + direction / 4, attackRange);
        foreach (Collider enemy in hitEnemies)
            if (enemy.tag == "enemy" && enemy.transform.root != transform)
                enemy.GetComponent<IDamageable>().Damage(5);
    }

    public void Damage(int damage)
    {
        Health -= damage;
    }

    private void CreateDeadBody()
    {
        PhotonNetwork.Destroy(this.gameObject);
        //create sprite
    }

    IEnumerator DamageColor()
    {
        _renderer.color = new Color32(153, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        _renderer.color = Color.white;
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
        Debug.Log(other.gameObject.tag);
        if (other.tag == "coin")
            Destroy(other.gameObject);
    }
}
