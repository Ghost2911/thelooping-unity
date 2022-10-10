using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName = "???";
    public Sprite locationImage;
    public bool isBoss = false;

    private void Start()
    {
        if (isBoss)
        {
            TargetPresentor targetMark = Inventory.instance.GetNextTargetPresentor();
            targetMark.SetSprite(locationImage);
            GetComponentInParent<EntityStats>().DeathEvent.AddListener(targetMark.SetCompleted);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            LocationPresentor.instance.ShowLocationName(locationName);
    }
}
