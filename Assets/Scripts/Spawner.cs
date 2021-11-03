using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxCount = 3;
    public Transform target;
    const float randomRange = 3f;
    
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && transform.childCount==0)
        {
            target = other.transform;
            CreateEnemy();
        }
    }

    public void CreateEnemy()
    {
        enemyPrefab.GetComponent<Unit>().target = target;
        Vector3 enemyPosition = transform.position + (transform.position - target.position);

        for (int i=transform.childCount; i<maxCount; i++)
            Instantiate(enemyPrefab, enemyPosition + new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange)),new Quaternion(0,0,0,0),transform);
    }


    private void OnDrawGizmos()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            target = null;
    }
}
