using UnityEngine;

public class ToxicBoots : Status
{
    public override void Tick()
    {
        Instantiate(Resources.Load("Explossion/toxic_explossion") as GameObject,
            transform.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)),
            new Quaternion(0, 0, 0, 0)).GetComponent<Explossion>().owner = transform;
    }
}

