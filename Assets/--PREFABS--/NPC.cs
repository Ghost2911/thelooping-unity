using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Message messageBubble;
    public string messageOnTakeDamage;

    public void MessageOnTakeDamage()
    {
        StopAllCoroutines();
        StartCoroutine(messageBubble.ShowText(messageOnTakeDamage));
        StartCoroutine(HitAnimation());
    }

    public void AnimationOnDeath()
    {
        StopAllCoroutines();
        StartCoroutine(DeathAnimation());
    }

    IEnumerator HitAnimation()
    {
        float startScale = transform.localScale.y;
        float endScale = transform.localScale.y / 3f;
        float time = 0;

        while (time < 1)
        {
            transform.localScale = new Vector3(transform.localScale.x,Mathf.Lerp(startScale,endScale,time),transform.localScale.z);
            time += Time.deltaTime*10;
            yield return null;
        }
        time = 0;
        while (time < 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(endScale, startScale, time), transform.localScale.z);
            time += Time.deltaTime*10;
            yield return null;
        }
    }

    public IEnumerator DeathAnimation()
    {
        float startScale = transform.localScale.y;
        float endScale = 0;
        float time = 0;

        while (time < 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(startScale, endScale, time), transform.localScale.z);
            time += Time.deltaTime * 5;
            yield return null;
        }
        Instantiate(Resources.Load("Particles/NPC_death") as GameObject,
            transform.position, new Quaternion(0, 0, 0, 0));
        Destroy(gameObject);
    }
}
