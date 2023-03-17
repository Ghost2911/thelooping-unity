using UnityEngine;

public class TotemPuzzle : MonoBehaviour
{
    public Totem[] totems;
    public SlotType rewardSlot;
    public GameObject destroyedObject;

    public void CheckActivation()
    {
        foreach (Totem totem in totems)
            if (!totem.isActive)
                return;

        if (destroyedObject == null)
            GlobalSettings.instance.CreateItem(rewardSlot, transform.position);
        else
            Destroy(destroyedObject);
    }
}
