using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChesserDialogue : MonoBehaviour
{
    public Animator leftText;
    public Animator rightText;

    void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        while (true)
        {
            ShowLeftText(3f);
            yield return new WaitForSeconds(1f);
            ShowRightText(2.5f);
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator ShowRightText(float duration)
    {
        rightText.SetTrigger("Show");
        yield return new WaitForSeconds(duration);
        rightText.SetTrigger("Hide");
    }

    IEnumerator ShowLeftText(float duration)
    {
        leftText.SetTrigger("Show");
        yield return new WaitForSeconds(duration);
        leftText.SetTrigger("Hide");
    }
}
