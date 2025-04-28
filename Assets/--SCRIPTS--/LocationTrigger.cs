using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LocationTrigger : MonoBehaviour
{
    public string locationName = "???";
    public Sprite locationImage;
    public AudioClip soundtrack;
    public Transform cameraPosition;
    public bool isBoss = false;
    public UnityEvent OnLocationEnter;
    public UnityEvent OnLocationExit;
    protected GameObject player;

    public void Start()
    {
        if (isBoss)
        {
            TargetPresentor targetMark = Inventory.instance.GetNextTargetPresentor();
            targetMark.SetSprite(locationImage);
            GetComponentInParent<EntityStats>().DeathEvent.AddListener(targetMark.SetCompleted);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            LocationPresentor.instance?.ShowLocationName(locationName);
            Statistic.instance?.OnEnterInArea(locationName);
            OnLocationEnter.Invoke();
            if (soundtrack != null)
                GlobalSettings.instance.ChangeBackgroundSoundtrack(soundtrack);
            if (cameraPosition != null)
                GlobalSettings.instance.SetCameraTraget(cameraPosition);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            OnLocationExit.Invoke();
            if (soundtrack != null)
                GlobalSettings.instance.ChangeBackgroundSoundtrack(null);
            if (cameraPosition != null)
                GlobalSettings.instance.SetCameraTraget(other.transform);
        }
    }
}
