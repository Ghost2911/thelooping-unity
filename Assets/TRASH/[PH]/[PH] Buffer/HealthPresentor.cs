using UnityEngine;

public class HealthPresentor : MonoBehaviour
{
    public Animator[] hpAnimator;

    void Awake()
    {
        PlayerInput.instance.HealthChangeEvent.AddListener(ChangeValuePresentor);
    }

    public void ChangeValuePresentor(int value)
    {
        int fullHeart = value / 2, halfHeart = value % 2;
        Debug.Log("full="+fullHeart + " half="+ halfHeart);
        for (int i = 0; i < fullHeart; i++)
            hpAnimator[i].SetTrigger("Full");
        if (halfHeart != 0)
            hpAnimator[fullHeart++].SetTrigger("Half");
        for (int i=fullHeart; i < hpAnimator.Length; i++)
            hpAnimator[i].SetTrigger("Empty");
    }
}
