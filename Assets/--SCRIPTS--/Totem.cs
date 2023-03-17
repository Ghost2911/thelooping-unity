using UnityEngine;
using UnityEngine.Events;

public class Totem : EntityStats
{
    [Header("Totem settings")]
    public StatusType statusActivate;
    public UnityEvent OnTotemActivate;
    public bool isActive = false;

    private void Start()
    {
        StatusTakeEvent.AddListener(CheckStatus);
    }

    [ContextMenu("Activate totem")]
    public void Activate()
    {
        isActive = true;
        animator.SetTrigger("Activate");
        OnTotemActivate.Invoke();
    }

    public void CheckStatus(StatusData status)
    {
        if (!isActive)
            if (status.type == statusActivate)
                Activate();
    }

    public void Deactivate()
    {
        GetComponent<Collider>().enabled = false;
        animator.enabled = false;
        Destroy(this);
    }
}
