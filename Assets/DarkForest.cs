using System.Collections;
using UnityEngine;

public class DarkForest : MonoBehaviour
{
    public Vector2 direction = new Vector2(0f, 0f);
    private Transform player;
    private Vector2 darknessPos;

    private void Awake()
    {
        darknessPos = V3toV2(transform.position) + new Vector2(24f, 24f) * direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            StartCoroutine(ChangeAmbientLight());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            StopCoroutine(ChangeAmbientLight());
        }
    }

    IEnumerator ChangeAmbientLight()
    {
        while (true)
        {
            Vector2 distance = (darknessPos - V3toV2(player.position)) * direction;
            float darkMultiplier = distance.magnitude / 35f;
            RenderSettings.ambientLight = Color.Lerp(Color.white, Color.black, darkMultiplier);
            if (darkMultiplier > 1.1f) 
                player.transform.position += V3toV2(direction)
                    *MapGenerator.instance.tileSize*(MapGenerator.instance.mapSize-0.5f);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public Vector2 V3toV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    public Vector3 V3toV2(Vector2 v2)
    {
        return new Vector3(v2.x, 0, v2.y);
    }
}