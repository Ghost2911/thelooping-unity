using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Vector3 direction;
    public float randomization = 0f;
    public float cooldown = 1f;
    private Transform _target;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(Attack());
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
     
    private void CreateBullet()
    {
        Vector3 randomVector = Vector3.Cross(direction, Vector3.up) * Random.Range(-randomization, randomization);
        GameObject bullet = Instantiate(projectilePrefab, transform.position, new Quaternion(0, 0, 0, 0));
        bullet.transform.LookAt(_target.position + randomVector, Vector3.up);
        bullet.transform.Rotate(new Vector3(90, -90, 0));
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            
            if (_target != null)
            {
                if (_animator != null)
                    _animator.SetTrigger("Fire");
                transform.localScale = new Vector3((_target.position.x > transform.position.x)?-1f:1f, 1f, 1f);
                CreateBullet();
            }
            yield return new WaitForSeconds(cooldown);
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && transform.childCount == 0)
            _target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _target = null;
    }
}
