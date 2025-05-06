using UnityEngine;

public class ExplossionShrapnel : Explossion
{
    public int numberShrapnel = 8; 
    public float radiusShrapnel = 1f;          
    public GameObject prefabShrapnel; 

    public void CreateShrapnel()
    {
        float rotateAngle = Mathf.PI * 2 / numberShrapnel;
        //prefabShrapnel.AddComponent<ArcFlight>();

        for (int i = 0; i < numberShrapnel; i++)
        {
            Vector3 targetPosition = transform.position + new Vector3(Mathf.Cos(i * rotateAngle) * radiusShrapnel, 0f, Mathf.Sin(i * rotateAngle) * radiusShrapnel);
            GameObject shrapnel = Instantiate(prefabShrapnel,  transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity);
            shrapnel.GetComponent<Collider>().enabled = false;
            shrapnel.AddComponent<ArcFlight>().target = targetPosition;
        }
    }
}
