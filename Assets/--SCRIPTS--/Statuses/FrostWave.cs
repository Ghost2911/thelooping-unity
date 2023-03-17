using UnityEngine;

public class FrostWave : Status
{
    private object explossion;
    private GameObject[] frostObjects = new GameObject[8];

    public override void Tick(){}

    public override void OnActivate()
    {
        explossion = Resources.Load("Explossion/ice_explossion");
        frostObjects[0] = Instantiate(explossion as GameObject,transform.position, Quaternion.identity);
        frostObjects[0].GetComponent<Wave>().Init(new Vector3(0f, 0f, 1f), transform);
        frostObjects[1] = Instantiate(explossion as GameObject, transform.position, Quaternion.identity);
        frostObjects[1].GetComponent<Wave>().Init(new Vector3(1f, 0f, 1f), transform);
        frostObjects[2] = Instantiate(explossion as GameObject, transform.position, Quaternion.identity);
        frostObjects[2].GetComponent<Wave>().Init(new Vector3(1f, 0f, 0f), transform);
        frostObjects[3] = Instantiate(explossion as GameObject, transform.position, Quaternion.identity);
        frostObjects[3].GetComponent<Wave>().Init(new Vector3(1f, 0f, -1f), transform);
        frostObjects[4] = Instantiate(explossion as GameObject, transform.position, Quaternion.identity);
        frostObjects[4].GetComponent<Wave>().Init(new Vector3(0f, 0f, -1f), transform);
        frostObjects[5] = Instantiate(explossion as GameObject, transform.position, Quaternion.identity);
        frostObjects[5].GetComponent<Wave>().Init(new Vector3(-1f, 0f, -1f), transform);
        frostObjects[6] = Instantiate(explossion as GameObject, transform.position, Quaternion.identity);
        frostObjects[6].GetComponent<Wave>().Init(new Vector3(-1f, 0f, 0f), transform);
        frostObjects[7] = Instantiate(explossion as GameObject, transform.position, Quaternion.identity);
        frostObjects[7].GetComponent<Wave>().Init(new Vector3(-1f, 0f, 1f), transform);
    }

    private void OnDisable()
    {
        foreach (GameObject go in frostObjects)
            Destroy(go);
    }
}
