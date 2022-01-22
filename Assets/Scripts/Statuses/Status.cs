using System.Collections;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    public int statusTime = 0;
    protected IDamageable target;

    private void Start()
    {
        target = GetComponent<IDamageable>();
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        OnRecieve();
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Tick();
            if (--statusTime == 0)
            {
                Tick();
                OnDestroy();
                Destroy(this);
            }
        }
    }

    public abstract void Tick();
    public virtual void OnRecieve() { }
    public virtual void OnDestroy() { }
}
