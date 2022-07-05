using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName = "???";
    private void OnTriggerEnter(Collider other)
    {
        LocationPresentor.instance.ShowLocationName(locationName);
    }
}
