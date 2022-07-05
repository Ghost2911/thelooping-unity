using System.Collections;
using TMPro;
using UnityEngine;

public class LocationPresentor : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    private Animator _animator;
    private Coroutine _cor = null;
    public static LocationPresentor instance;
    
    void Start()
    {
        instance = this;
        _animator = GetComponent<Animator>();
        _textMesh = GetComponent<TextMeshProUGUI>();
        _animator.enabled = false;
        _textMesh.enabled = false;
    }

    public void ShowLocationName(string locationName)
    {
        if (_cor == null)
        {
            _textMesh.text = "~ " + locationName + " ~";
            _cor = StartCoroutine(ActivateAnimator());
        }
    }

    IEnumerator ActivateAnimator()
    {
        _animator.enabled = true;
        _textMesh.enabled = true;
        _animator.Play(0);
        yield return new WaitForSeconds(4f);
        _animator.enabled = false;
        _textMesh.enabled = false;
        _cor = null;
    }
}
