using UnityEngine;
using UnityEngine.Events;

public class Statue : MonoBehaviour, IDamageable
{   
    public UnityEvent<int> StatueActivateEvent;
    public int Health { get; set; }
    public bool canRepeatActivate = false;
    private GameObject activateAura;  

    public int number; 
    [HideInInspector]
    public bool isActive = false;

    private void Awake()
    {
        activateAura = transform.GetChild(0).gameObject;
        SetStatueStatus(false);
    }

    public void Damage(int damage,float knockbackPower,Vector3 direction,Color blindColor, EntityStats damageSource = null, bool ignoreArmor = false)
    {
        if (!isActive || canRepeatActivate)
        {
            SetStatueStatus(true);
            StatueActivateEvent.Invoke(number);
        }
    }

    public void SetStatueStatus(bool _isActive)
    {
        activateAura.SetActive(_isActive);
        isActive = _isActive;
    }
}
