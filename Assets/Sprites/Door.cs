using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject linkedDoor;


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("stay - "+other.name);
        if (other.CompareTag("Player"))
        {   
            other.transform.position = linkedDoor.transform.position - linkedDoor.transform.forward*4;
        }
    }
}
