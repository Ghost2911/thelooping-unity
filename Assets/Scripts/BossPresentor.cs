using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class BossPresentor : MonoBehaviour
{
    public TextMeshProUGUI locationPresentor;
    public string locationName;
    public UnityEvent<Transform> TargetChangeEvent;

    public Animator _animator;

    void Awake()
    {
        _animator = locationPresentor.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(Present());
            TargetChangeEvent.Invoke(col.transform);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            TargetChangeEvent.Invoke(null);
        }
    }

    IEnumerator Present()
    {
        locationPresentor.text = locationName;
        _animator.SetTrigger("ShowText");
        yield return new WaitForSeconds(2f);
        _animator.SetTrigger("HideText");
    }
}
