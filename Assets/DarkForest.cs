using System.Collections;
using UnityEditor;
using UnityEngine;

public class DarkForest : MonoBehaviour
{
    public enum Axis { horizontal, vertical, diagonal};
    public Axis direction = Axis.horizontal;
    private Transform player;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            player = other.transform;
            StartCoroutine(ChangeAmbientLight());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            player = null;
        }
    }

    IEnumerator ChangeAmbientLight()
    {
        while (true)
        {
            float distance;
            switch (direction)
            {
                case Axis.horizontal:
                    distance = transform.position.x - player.position.x + 24;
                    break;
                case Axis.vertical:
                    distance = transform.position.z - player.position.z + 24;
                    break;
                default:
                    distance = 0;
                    break;
            }
            RenderSettings.ambientLight = Color.Lerp(Color.white, Color.black, (48f - distance)/40f);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    /*
    IEnumerator BlackColor()
    {
        float t = 0;
        while (t!=1)
        {
            t += 0.01f;
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Color.black, t);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator WhiteColor()
    {
        float t = 0;
        while (t != 1)
        {
            t += 0.01f;
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Color.white, t);
            yield return new WaitForSeconds(0.1f);
        }
    }*/

}
