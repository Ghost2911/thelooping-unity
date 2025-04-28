using UnityEngine;
using System.Collections;

public class MoonObelisk : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public Sprite destroyObject;
    public GameObject skull;
    public int cooldown = 10;
    public int hitCount = 1;
    private SpriteRenderer _renderer;
    protected Collider _collider;
    private bool dropRecieved = false;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();
    }

    public bool GiveSkull()
    {
        if (skull.activeSelf)
        {
            StartCoroutine("GiveSkullCoroutine");
            return true;
        }
        return false;
    }

    IEnumerator GiveSkullCoroutine()
    {
        skull.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        skull.SetActive(true);
    }

    public void Damage(HitInfo hitInfo)
    {
        if (!dropRecieved)
        {
            hitCount--;
            if (hitCount <= 0)
            {
                StopAllCoroutines();
                
                cooldown = 999;
                skull.SetActive(true);
                skull.GetComponent<ProjectilePuncture>().InitialSetup(transform.position, transform);

                Statistic.instance?.OnDestroyObject(hitInfo.damageSource?.name,gameObject.name);
                if (destroyObject != null)
                    _renderer.sprite = destroyObject;
                else
                {
                    skull.transform.SetParent(null);    
                    Destroy(gameObject);
                }
                dropRecieved = true;
            } 
        }
    }
}