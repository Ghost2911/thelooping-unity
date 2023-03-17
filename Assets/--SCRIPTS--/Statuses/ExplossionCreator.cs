using UnityEngine;

public class ExplossionCreator : Status
{
    public override void Tick()
    {
        Instantiate(statusData.additiveObject,
            transform.position + new Vector3(Random.Range(-statusData.range, statusData.range), 0f, Random.Range(-statusData.range, statusData.range)),
            new Quaternion(0, 0, 0, 0)).GetComponent<Explossion>().owner = transform;
    }
}

