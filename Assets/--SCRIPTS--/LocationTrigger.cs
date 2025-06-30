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
            if (PlayerPrefs.GetInt(GetComponentInChildren<EntityStats>().name, 0) == 0)
            {
                //TargetPresentor targetMark = Inventory.instance.GetNextTargetPresentor();
                //targetMark.SetSprite(locationImage);
                //GetComponentInParent<EntityStats>().DeathEvent.AddListener(targetMark.SetCompleted);
                GetComponentInChildren<EntityStats>().DeathEvent.AddListener(this.SaveBossDeath);
            }
            else
            {
                gameObject.SetActive(false);
            }
            Debug.Log("Загружен босс " + GetComponentInChildren<EntityStats>().name + " | Состояние: " + PlayerPrefs.GetInt(GetComponentInChildren<EntityStats>().name, 0));
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

    public void SaveBossDeath()
    {
        Debug.Log("Сохранен босс "+GetComponentInChildren<EntityStats>().name);
        PlayerPrefs.SetInt(GetComponentInChildren<EntityStats>().name, 1);
        PlayerPrefs.Save();
    }
}
