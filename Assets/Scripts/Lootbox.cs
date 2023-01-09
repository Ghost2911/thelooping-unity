using UnityEngine;

public class Lootbox : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public GameObject[] drops;
    public Sprite destroyObject;
    public int hitCount = 1;
    public bool randomizeDrop = false;
    private SpriteRenderer _renderer;
    protected Collider _collider;
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
            hitCount--;
            for (int i=0;i<drops.Length;i++)
            {
                if (Random.Range(0,2)>0 || !randomizeDrop)
                    Instantiate(drops[i], new Vector3(transform.position.x, 0.05f, transform.position.z), drops[i].transform.rotation);
            }

            if (hitCount <= 0)
            {
                Statistic.instance?.OnDestroyObject(damageSource?.name,gameObject.name);
                if (destroyObject != null)
                    _renderer.sprite = destroyObject;
                else
                    Destroy(gameObject);

                dropRecieved = true;
            } 
        }
    }
}