using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParentTest : MonoBehaviour
{
    public Transform destination;


    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, destination.position, Time.deltaTime * 5f);
    }

}
