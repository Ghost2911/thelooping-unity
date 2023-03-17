using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject linkedDoor;
    public bool exitInFront = true;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("stay - "+other.name);
        if (other.CompareTag("Player"))
        {   
            other.transform.position = linkedDoor.transform.position - ((exitInFront) ?linkedDoor.transform.forward:-linkedDoor.transform.forward)*2.5f;
        }
    }
}
