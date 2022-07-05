using UnityEngine;

public interface IDamageable
{
    int Health { get; set; }
    void Damage( int damage, float knockbackPower, Vector3 direction, UnityEngine.Color blindColor, EntityStats damageSource=null);
}
