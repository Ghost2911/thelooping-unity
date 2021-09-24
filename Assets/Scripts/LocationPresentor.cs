using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationPresentor : MonoBehaviour
{
    public Text locationPresentor;
    public string locationName;

    private Animator _animator;

    void Awake()
    {
        _animator = locationPresentor.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log(locationPresentor.color.a==0);
            if (locationPresentor.color.a==0)
            {
                locationPresentor.text = locationName;
                _animator.SetTrigger("Activate");
            }
        }
    }

    
}
