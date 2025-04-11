using System.Collections;
using UnityEngine;

public class Stun : Status
{
    private Vector3 startScale;
    public override void OnActivate()
    {
        startScale = target.transform.localScale;
        //StartCoroutine(HitAnimation(target.transform));
        target.isStunned = true;
        targetAnimator.speed -= 1;
    }
    void OnDisable()
    {
        StopAllCoroutines();
        target.isStunned = false;
        targetAnimator.speed +=1;
        target.transform.localScale = startScale;
    }
    
    public override void Tick() { }

    /*
    IEnumerator HitAnimation(Transform target)
    {
        
        float startScale = target.localScale.y;
        float time = 0;

        while (true)
        {
            transform.localScale = new Vector3(target.localScale.x, startScale + Mathf.Sin(time)/4, target.localScale.z);
            time += Time.deltaTime * 70;
            yield return null;
        }  
    }
    */
}