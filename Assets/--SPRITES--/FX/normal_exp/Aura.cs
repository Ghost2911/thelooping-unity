using UnityEngine;

public class Aura : MonoBehaviour
{
    public StatusData status;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<EntityStats>()?.AddStatus(status);
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<EntityStats>()?.RemoveStatus(status);
    }
}
