using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusLink : MonoBehaviour
{
    public string relatedObjectTag = "deadbody";
    protected List<Link> links = new List<Link>();
    protected Transform holder;
    protected float colliderRadius = 0f;

    void Start()
    {
        links.AddRange(GetComponentsInChildren<Link>());
        holder = transform.parent;
        colliderRadius = GetComponent<SphereCollider>().radius;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(relatedObjectTag))
        {
            foreach (Link l in links)
                if (l.endPosition == null)
                {
                    l.SetPosition(holder, other.transform);
                    break;
                }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag(relatedObjectTag))
            {
                foreach (Link l in links)
                    if (l.endPosition != null)
                        if (l.distance > colliderRadius + 2f)
                            l.ClearPosition();
            }
        }
    }
}
