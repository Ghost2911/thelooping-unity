using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableLootbox : Lootbox, IUsable
{
    private bool isUse = false;
    
    public void Use(EntityStats entity)
    {
        if (!isUse)
        {
            entity.usableItem = this;
            _collider.enabled = false;
            transform.SetParent(entity.usableItemSlot);
            transform.localPosition = new Vector3(0, 0, 0);
            transform.rotation = entity.usableItemSlot.rotation;
            transform.GetComponentInChildren<Collider>().enabled = false;
        }
        else
        {
            entity.usableItem = null;
            transform.SetParent(null);
            _collider.enabled = true;
            gameObject.AddComponent<ArcFlight>().target = entity.transform.position + entity.direction * 5f;
        }
        isUse = !isUse;
    }
}
