 using UnityEngine;

public class FireHand : Status
{
    private GameObject explossion;

    public override void OnActivate()
    {
        explossion = Resources.Load("Explossion/FireHand") as GameObject;
    }

    public override void Tick()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 25f);
        explossion.transform.localScale = new Vector3(target.transform.localScale.x,1,1);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<IStatusable>()!=null && enemy.transform != target.transform)
            {
                Instantiate(explossion, enemy.transform.position, Quaternion.identity).GetComponent<Explossion>().owner = transform;
            }
        }
    }
}