using System.Collections;
using UnityEngine;
using TMPro;

public class Message : MonoBehaviour
{
    [TextArea]
    public string[] Messages;

    private float duration = 5f;
    private TextMeshPro tm;
    private int messageNum = 0;
    private Animator _animator;
    private bool isActive = false;
    private Coroutine corutShowText = null;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        tm = GetComponent<TextMeshPro>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            if (corutShowText == null)
                corutShowText = StartCoroutine(ShowText());
            isActive = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;
    }

    IEnumerator ShowText()
    {
        while (true)
        {
            tm.text = NextMessage();
            _animator.SetTrigger("ShowText");
            yield return new WaitForSeconds(duration);
            _animator.SetTrigger("HideText");
            yield return new WaitForSeconds(1f);
            if (!isActive)
                break;
        }
        corutShowText = null;
    }

    string NextMessage()
    {
        if (messageNum >= Messages.Length)
            messageNum = 0;
        return Messages[messageNum++];
    }
    
}
