using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnEnemy;
    public float spawnTime = 3f;
    public int maxSpawnCount = 3;
    private float radius;
    private Unit[] units;
    private Turret[] turrets;
    private Transform target;
    private bool preGeneration = false;

    private void Awake()
    {
        if (transform.childCount != 0)
        {
            units = transform.GetComponentsInChildren<Unit>();
            turrets = transform.GetComponentsInChildren<Turret>();
            preGeneration = true;
        }
        radius = transform.GetComponent<CapsuleCollider>().radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
            if (preGeneration)
            {
                foreach (Unit unit in units)
                    if (unit != null)
                        unit.SetTarget(target);
                foreach (Turret turret in turrets)
                    turret.SetTarget(target);
            }
            else
            {
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            target = null;
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            if (maxSpawnCount > transform.childCount)
            {
                Vector3 offset = new Vector3(Mathf.Cos(Random.Range(0, Mathf.PI * 2)), 0f, Mathf.Sin(Random.Range(0, Mathf.PI * 2))) * radius;
                Unit unit = Instantiate(spawnEnemy, transform.position + offset, Quaternion.identity, transform).GetComponent<Unit>();
                unit.SetTarget(target);
            }
        }
    }
}
