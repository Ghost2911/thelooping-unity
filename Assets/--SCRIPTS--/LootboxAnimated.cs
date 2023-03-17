using UnityEngine;

public class LootboxAnimated : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public GameObject[] drops;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
    }

    public void Damage(HitInfo hitInfo)
    {
        _animator.enabled = false;
        foreach (GameObject drop in drops)
            Instantiate(drop, transform.position + new Vector3(0, 0, -0.05f), Quaternion.identity);
        _animator.SetTrigger("Hit");
        Destroy(gameObject, 0.2f);
    } 
}
