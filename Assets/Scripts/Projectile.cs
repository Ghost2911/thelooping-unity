using UnityEngine;
public class Projectile : MonoBehaviour, IThrowable
{
    public int damage;
    public float range;
    public Vector3 destination;
    public float speed = 10f;
    public float offset = 2f;
    public Transform owner;
    public float scatter;
    public StatusData status;

    void Start()
    {
        transform.position += new Vector3(0,offset,0);
        destination = transform.position + transform.right * range + Random.Range(-scatter, scatter) * transform.up;
    }

    public void InitialSetup(Vector3 target, Transform owner)
    {
        transform.LookAt(target,Vector3.up);
        transform.Rotate(new Vector3(90f, -90f, 0f), Space.Self);
        destination = target;
        this.owner = owner;
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
        if (other.transform != owner && !other.isTrigger)
        {
            if (other.GetComponent<IDamageable>()!=null) 
                other.GetComponent<IDamageable>().Damage(damage, 0f, Vector3.zero, Color.red);
            Destroy(this.gameObject);
        }
    }
}
