using System.Collections;
using UnityEngine;

public class Liquid : Trap, IStatusable
{
    public LiquidState state;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        transform.localScale = Vector3.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LiquidResizeTo(new Vector3(2, 2, 2)));
        ChangeState(state);
    }

    private void ChangeState(LiquidState liquidState)
    {
        state = liquidState;
        spriteRenderer.sprite = liquidState.sprite;
        spriteRenderer.color = liquidState.color;
        trapStatus = liquidState.statusOnStay;

        if (liquidState.statusOnStart != null)
            AddOnStartStatus(liquidState.statusOnStart);
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
