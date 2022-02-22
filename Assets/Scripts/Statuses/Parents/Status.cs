using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Status : MonoBehaviour
{
    public StatusData statusData;
    public float duration; 
    protected EntityStats target;
    protected Animator statusAnimator;
    protected Animator targetAnimator;

    private void Start()
    {
        target = GetComponent<EntityStats>();
        targetAnimator = GetComponent<Animator>();
        statusAnimator = transform.GetChild(0).GetComponent<Animator>();
        statusAnimator.runtimeAnimatorController = statusData.animator;
        duration = statusData.duration;
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    { 
        OnStatusEnable();
        while (true)
        {
            yield return new WaitForSeconds(statusData.deltaTick);
            Tick();
            if (--duration == 0)
            {
                OnStatusDisable();
                Destroy(this);
            }
        }
    }

    public abstract void Tick();
    public abstract void OnStatusEnable();
    public abstract void OnStatusDisable();
}
