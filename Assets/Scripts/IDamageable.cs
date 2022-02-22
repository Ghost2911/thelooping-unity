public interface IDamageable
{
    int Health { get; set; }
    void Damage(int damage, UnityEngine.Color blindColor);
}
