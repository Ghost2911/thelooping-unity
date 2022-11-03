using UnityEngine;

public class Lootbox : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public GameObject[] drops;
    public Sprite destroyObject;
    private SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Damage(int damage, float knockbackPower, Vector3 direction, Color blindColor, EntityStats damageSource = null, bool ignoreArmor = false)
    {
        foreach (GameObject drop in drops)
            Instantiate(drop, transform.position + new Vector3(0,0,-0.05f), drop.transform.rotation);

        if (_renderer != null)
        {
            _renderer.sprite = destroyObject;
            Destroy(this);
        }
        else
            Destroy(gameObject);
    }
}