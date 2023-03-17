using UnityEngine;

public class Lootbox : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public GameObject[] dropOnHit;
    public GameObject[] dropOnDestroy;
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

    public void Damage(HitInfo hitInfo)
    {
        if (!dropRecieved)
        {
            hitCount--;
            for (int i=0;i<dropOnHit.Length;i++)
            {
                if (Random.Range(0,2)>0 || !randomizeDrop)
                    Instantiate(dropOnHit[i], new Vector3(transform.position.x, 0.05f, transform.position.z), dropOnHit[i].transform.rotation);
            }

            if (hitCount <= 0)
            {
                Statistic.instance?.OnDestroyObject(hitInfo.damageSource?.name,gameObject.name);
                if (destroyObject != null)
                    _renderer.sprite = destroyObject;
                else
                    Destroy(gameObject);
                
                for (int i=0;i<dropOnDestroy.Length;i++)
                    Instantiate(dropOnDestroy[i], new Vector3(transform.position.x, 0.05f, transform.position.z), dropOnDestroy[i].transform.rotation);
                dropRecieved = true;
            } 
        }
    }
}