using UnityEngine;
using UnityEngine.Events;

public class Bell : MonoBehaviour
{
    public UnityEvent<int> BellActivateEvent;
    public Animator animator;
    public int number;

    private void OnMouseUp()
    {
        animator.Play(0);
        BellActivateEvent.Invoke(number);
    }
}
