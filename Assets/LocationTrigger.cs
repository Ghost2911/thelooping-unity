using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName = "???";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            LocationPresentor.instance.ShowLocationName(locationName);
    }
}
