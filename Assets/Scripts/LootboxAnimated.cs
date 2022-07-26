using UnityEngine;

public class LootboxAnimated : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public GameObject[] drops;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Damage(int damage, float knockbackPower, Vector3 direction, Color blindColor, EntityStats damageSource=null, bool ignoreArmor = false)
    {
        foreach (GameObject drop in drops)
            Instantiate(drop, transform.position, new Quaternion(0f, 0f, 0f, 0f));
        _animator?.SetTrigger("Hit");
        Destroy(this);
    } 

}
