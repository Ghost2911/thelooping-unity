using System.Collections;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    public StatusData statusData;
    public float duration;
    public int layer;
    protected EntityStats target;
    protected Animator statusAnimator;
    protected Animator targetAnimator;

    private void Start()
    {
        if (statusData != null)
            Init(statusData);
    }

    public void Init(StatusData status)
    {
        statusData = status;
        
        duration = statusData.duration;
        targetAnimator = GetComponent<Animator>();
        if (transform.childCount!=0)
            statusAnimator = transform.GetChild(status.layer).GetComponent<Animator>();
        target = GetComponent<EntityStats>();
        if (target!=null)
            target.statusEffects.Add(this);
        if (statusAnimator!=null)
        {
            statusAnimator.runtimeAnimatorController = statusData.animator;
            statusAnimator.Play("StatusStart", 0);
        }
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
