using UnityEngine;

public class FrogRain : Status
{
    public override void Tick()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 15f);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                Instantiate(Resources.Load("Explossion/frogRain") 
                    as GameObject,enemy.transform.position, Quaternion.identity).GetComponent<Explossion>().owner = transform;
            }
        }
    }
}
