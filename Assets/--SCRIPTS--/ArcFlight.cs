using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcFlight : MonoBehaviour
{
    public Vector3 target;
    public DamageType damageType = DamageType.Impact;
    public float speed = 3;
    private float time = 0;
    private Vector3 startPos;

    private void Start()
    {
        transform.Rotate(new Vector3(0, 0, 15));
        startPos = transform.position;
    }


    void Update()
    {
        if (target != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, target) == 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.GetComponentInChildren<Collider>().enabled = true;
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 0.5f);

                foreach (Collider enemy in hitEnemies)
                {
                    IDamageable damagable = enemy.GetComponent<IDamageable>();
                    damagable?.Damage(new HitInfo(damageType,5,0f,Vector3.zero,Color.red));
                }
                Destroy(this);
                return;
            }
            transform.position = Vector3.Lerp(startPos, target, time);
            time += Time.deltaTime * speed;
        }
    }
}
