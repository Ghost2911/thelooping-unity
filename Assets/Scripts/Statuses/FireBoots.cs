using UnityEngine;

public class FireBoots : Status
{
    public override void Tick()
    {
        Instantiate(Resources.Load("Explossion/fire_explossion") as GameObject, 
            transform.position + new Vector3(Random.Range(-2f, 2f), 0f,Random.Range(-2f,2f)), 
            new Quaternion(0, 0, 0, 0)).GetComponent<Explossion>().owner = transform;
    }
}
