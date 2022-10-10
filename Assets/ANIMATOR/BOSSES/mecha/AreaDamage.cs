using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    public int areaDamage = 5;
    public StatusData status;
    public float lifetime = -1;
    public List<GameObject> targets;
    private Coroutine _cor;

    private void OnTriggerEnter(Collider other)
    {
        targets.Add(other.gameObject);
        if (_cor==null)
        {
            _cor = StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject);
    }

    IEnumerator Attack()
    {
        while (true)
        {
            foreach (GameObject target in targets)
            {
                if (target != null)
                {
                    target.GetComponent<EntityStats>()?.Damage(areaDamage, 0f, Vector3.zero, Color.red);
                    if (status!=null)
                        target.GetComponent<EntityStats>()?.AddStatus(status);
                }
                else
                    targets.Remove(target);
            }
            yield return new WaitForSeconds(0.5f);

            if (targets.Count == 0)
            {
                _cor = null;
                yield break;
            }
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        if (lifetime!=-1)
            Destroy(gameObject,lifetime);
        _cor = StartCoroutine(Attack());
        targets.Clear();
    }
}
