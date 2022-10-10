using System.Collections;
using UnityEngine;

public class WindSpeed : Status
{
    float speedUp = 0.7f;
    bool isChanged = false;

    public override void Tick()
    {
        statusAnimator.SetTrigger("Tick");
    }

    public override void OnActivate()
    {
        StartCoroutine(ChangeSpeedMultiplier());
    }

    IEnumerator ChangeSpeedMultiplier()
    {
        while (true)
        {
            target.speedMultiplier += speedUp;
            isChanged = true;
            yield return new WaitForSeconds(4);
            target.speedMultiplier -= speedUp;
            isChanged = false;
            yield return new WaitForSeconds(statusData.deltaTick);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(ChangeSpeedMultiplier());
        if (isChanged)
            target.speedMultiplier -= speedUp;
    }
}
