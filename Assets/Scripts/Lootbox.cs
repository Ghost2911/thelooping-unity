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

    public void Damage(int damage, float knockbackPower, Vector3 direction, Color blindColor, EntityStats damageSource = null)
    {
        foreach (GameObject drop in drops)
            Instantiate(drop, transform.position, Quaternion.identity);
        _renderer.sprite = destroyObject;
        Destroy(this);
    }

}