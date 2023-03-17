using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public GameObject particlePrefab;
    public int size = 5;
    public static ParticlePool instance;
    private ParticleSystem[] particles;
    public Material particleMaterial;
    private byte cursor = 0;

    private void Awake() 
    {
        instance = this;
        particles = new ParticleSystem[size];
        particlePrefab.GetComponent<ParticleSystemRenderer>().material.color = Color.green;
        for (int i=0; i<size;i++)
        {
            particles[i] = Instantiate(particlePrefab,transform).GetComponent<ParticleSystem>();
            particles[i].gameObject.SetActive(false);
        }
    }

    public void ChangeParticleMaterial(Material mat) 
    {
        if (mat!=null)
        particleMaterial = mat;
    }

    public void GetParticle(Vector3 position)
    {
        particles[cursor].gameObject.SetActive(true);
        particles[cursor].transform.position = position;
        cursor++;
        if (cursor >=size)
            cursor = 0;
    }

}
