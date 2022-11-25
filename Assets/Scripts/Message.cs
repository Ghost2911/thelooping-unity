using System.Collections;
using UnityEngine;
using TMPro;

public class Message : MonoBehaviour, IUsable
{
    [TextArea]
    public string[] Messages;
    [HideInInspector]
    public int messageNum = 0;
    public bool isAutoShow = true;

    private float duration = 5f;
    private TextMeshPro tm;
    private Animator _animator;
    protected bool isActive = false;
    protected Coroutine corShowText = null;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        tm = GetComponent<TextMeshPro>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            if (corShowText == null)
                corShowText = StartCoroutine(ShowText());
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;
    }

    public virtual IEnumerator ShowText()
    {
        if (corShowText != null)
        {
            _animator.SetTrigger("HideText");
            yield return new WaitForSeconds(1f);
        }
        do
        {
            tm.text = NextMessage();
            _animator.SetTrigger("ShowText");
            yield return new WaitForSeconds(duration);
            _animator.SetTrigger("HideText");
            yield return new WaitForSeconds(1f);
            if (!isActive)
                break;
        } while (isAutoShow);
        corShowText = null;
    }

    public virtual IEnumerator ShowText(string text)
    {
        if (corShowText != null)
        {
            _animator.SetTrigger("HideText");
            yield return new WaitForSeconds(1f);
        }
        tm.text = text;
        _animator.SetTrigger("ShowText");
        yield return new WaitForSeconds(duration);
        _animator.SetTrigger("HideText");
        yield return new WaitForSeconds(1f);
        corShowText = null;
    }


    string NextMessage()
    {
        if (messageNum >= Messages.Length)
            messageNum = 0;
        return Messages[messageNum++];
    }

    public void Use(EntityStats entity)
    {
        isActive = false;
        StopAllCoroutines();
        corShowText = StartCoroutine(ShowText());
    }
}
