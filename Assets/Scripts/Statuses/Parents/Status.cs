using System.Collections;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    public StatusData statusData;
    public float duration;
    protected EntityStats target;
    protected Animator statusAnimator;
    protected Animator targetAnimator;

    public void Init(StatusData status, int statusLayer)
    {
        statusData = status;
        target = GetComponent<EntityStats>();
        duration = statusData.duration;
        targetAnimator = GetComponent<Animator>();
        statusAnimator = transform.GetChild(statusLayer).GetComponent<Animator>();
        target.statusEffects.Add(this);
        if (statusAnimator!=null)
            statusAnimator.runtimeAnimatorController = statusData.animator;
        statusAnimator.Play("StatusStart", 0);
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

    public virtual void OnActivate(){}

    private void OnDestroy()
    {
        if (statusAnimator.runtimeAnimatorController == statusData.animator)
            statusAnimator.runtimeAnimatorController = null;
        target.statusEffects.Remove(this);
    }

    public abstract void Tick();
}
