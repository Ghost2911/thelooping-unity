using System.Collections;
using UnityEngine;

public class MechaArm : MonoBehaviour
{
    public int damage;
    public DamageType damageType = DamageType.Impact;
    public Transform _target;
    public float speed = 20f;
    public Vector3 offset;
    private Vector3 _startPos;

    void OnEnable()
    {
        _target = GetComponentInParent<Unit>().target.transform;
        _startPos = transform.position;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    { 
        float timer = 1f;
        while (timer > 0)
        {
            transform.position -= transform.right * speed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y, _target.position.z) + offset;

        timer = 2f;
        while (timer > 0)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        transform.position = _startPos;
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<IDamageable>() != null)
                other.GetComponent<IDamageable>().Damage(new HitInfo(damageType,damage, 0f, Vector3.zero, Color.red));
        }
    }
}
