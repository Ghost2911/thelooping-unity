using UnityEngine;

public class Lootbox : MonoBehaviour, IDamageable, IUsable
{
    public int Health { get; set; }
    public GameObject[] drops;
    public Sprite destroyObject;
    private SpriteRenderer _renderer;
    private Collider _collider;
    private bool isUse = false;
    private bool dropRecieved = false;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();
    }

    public void Damage(int damage, float knockbackPower, Vector3 direction, Color blindColor, EntityStats damageSource = null, bool ignoreArmor = false)
    {
        if (!dropRecieved)
        {
            foreach (GameObject drop in drops)
                Instantiate(drop, new Vector3(transform.position.x, 0.05f, transform.position.z), drop.transform.rotation);

            if (destroyObject != null)
            {
                Statistic.instance?.OnDestroyObject(damageSource?.name,gameObject.name);
                _renderer.sprite = destroyObject;
            }

            dropRecieved = true;
        }
    }

    public void Use(EntityStats entity)
    {
        if (!isUse)
        {
            entity.usableItem = this;
            _collider.enabled = false;
            transform.SetParent(entity.usableItemSlot);
            transform.localPosition = new Vector3(0, 0, 0);
            transform.rotation = entity.usableItemSlot.rotation;
            transform.GetComponentInChildren<Collider>().enabled = false;
        }
        else
        {
            entity.usableItem = null;
            transform.SetParent(null);
            _collider.enabled = true;
            gameObject.AddComponent<ArcFlight>().target = entity.transform.position + entity.direction * 5f;
        }
        isUse = !isUse;
    }
}