using UnityEngine;

public enum DamageType
{
    Impact, Slash, Fire, Toxin, Frost, Electricity
}

public class HitInfo
{
    public DamageType damageType;
    public int damageValue;
    public float knockbackPower; 
    public Vector3 direction;
    public Color blindColor;
    public EntityStats damageSource = null;
    public bool ignoreArmor = false;
    
    public HitInfo(DamageType damageType, int damageValue, float knockbackPower, Vector3 direction, Color blindColor, EntityStats damageSource=null, bool ignoreArmor=false)
    {
        this.damageType = damageType;
        this.damageValue = damageValue;
        this.knockbackPower = knockbackPower;
        this.direction = direction;
        this.blindColor = blindColor;
        this.damageSource = damageSource;
        this.ignoreArmor = ignoreArmor;
    }
}