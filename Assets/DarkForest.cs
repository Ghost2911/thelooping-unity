using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkForest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.ambientLight = Color.black;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.ambientLight = Color.white;
        }
    }
}
