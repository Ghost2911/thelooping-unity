using UnityEngine;

public class FireBoots : Status
{
    public override void Tick()
    {
        Instantiate(Resources.Load("Explossion/fire_explossion") as GameObject, 
            transform.position + new Vector3(Random.Range(-1f, 1f), 0f,Random.Range(-1f,1f)), 
            new Quaternion(0, 0, 0, 0)).GetComponent<Explossion>().owner = transform;
    }
}
