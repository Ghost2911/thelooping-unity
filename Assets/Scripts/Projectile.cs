using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    public float range;
    public Vector3 destination;
    public float speed = 10f;

    void Start()
    {
        destination = transform.position + transform.up * range;
    }

    void Update()
    {
        if (transform.position != destination)
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        else
            Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.transform.root != transform)
            other.GetComponent<PhotonView>().RPC("DealDamage", RpcTarget.All, 5);
    }
}
