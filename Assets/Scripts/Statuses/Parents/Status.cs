using System.Collections;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    public StatusData statusData;
    public float duration; 
    protected EntityStats target;
    protected Animator statusAnimator;
    protected Animator targetAnimator;

    public void Init()
    {
        target = GetComponent<EntityStats>();
        duration = statusData.duration;
        targetAnimator = GetComponent<Animator>();
        statusAnimator = transform.GetChild(0).GetComponent<Animator>();
        if (statusAnimator!=null)
            statusAnimator.runtimeAnimatorController = statusData.animator;
        StartCoroutine(Activate());
        OnActivate();
    }

    private IEnumerator Activate()
    { 
        while (true)
        {
            yield return new WaitForSeconds(statusData.deltaTick);
            Tick();
            if (--duration == 0)
                Destroy(this);
        }
    }

    public virtual void OnActivate()
    { }

    public abstract void Tick();
}
