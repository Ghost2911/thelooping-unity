using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : Trap, IStatusable
{
    public LiquidState state;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private List<Liquid> contactLiquids = new List<Liquid>();

    void Start()
    {
        transform.localScale = Vector3.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartCoroutine(LiquidResizeTo(new Vector3(2, 2, 2)));
        ChangeState(state);
    }

    private void ChangeState(LiquidState liquidState)
    {
        state = liquidState;
        
        spriteRenderer.sprite = liquidState.sprite;
        spriteRenderer.color = liquidState.color;
        animator.runtimeAnimatorController = liquidState.animator;
        trapStatus = liquidState.statusOnStay;

        if (Time.timeSinceLevelLoad > 1f)
            if (liquidState.statusOnStart != null)
                AddOnStartStatus(liquidState.statusOnStart);

        if (liquidState.statusOnStay != null)
            if (contactLiquids.Count != 0)
                foreach (Liquid liquid in contactLiquids)
                    liquid.AddStatus(liquidState.statusOnStay);
    }


    IEnumerator LiquidResizeTo(Vector3 size)
    {
        float percent = 0;
        while (percent < 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, size, percent);
            yield return new WaitForSeconds(0.01f);
            percent += Time.deltaTime;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("liquid"))
        {
            other.GetComponent<Liquid>().AddStatus(state.statusOnStay);
            contactLiquids.Add(other.GetComponent<Liquid>());
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("liquid"))
        {
            contactLiquids.Remove(other.GetComponent<Liquid>());
        }
    }

    public void AddOnStartStatus(StatusData statusData)
    {
        System.Type statusType = System.Type.GetType(statusData.type.ToString());
        Status status = gameObject.AddComponent(statusType) as Status;
        status.Init(statusData);
    }

    public void AddStatus(StatusData statusData)
    {
        foreach (LiquidState liquidVariation in state.stateTransitionOptions)
            if (liquidVariation.statusTransitionCondition == statusData)
                ChangeState(liquidVariation);
    }
}
