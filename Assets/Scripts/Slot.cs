using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class Slot : MonoBehaviour
{
    public ItemStats itemStats;
    public SlotType type;
    public UnityEvent<StatsType,int> StatsChangeEvent;
    public EntityStats stats;
    public Image image;
    public GameObject dropItem;

    private Sprite _imageEmpty;

    public void Awake()
    {
        _imageEmpty = image.sprite;
        if (dropItem == null)
            Debug.Log($"Slot {type.ToString()} not contain empty drop-container");
    }

    public void Equip(ItemStats equipItem)
    {
        if (itemStats != null)
            Unequip();

        itemStats = equipItem;
        image.sprite = itemStats.icon;
        StatsChangeEvent.Invoke(itemStats.type, itemStats.additiveValue);
        if (equipItem.status!=null)
            if (equipItem.status.useOnSelf)
                stats.AddStatus(equipItem.status);
    }

    public void Unequip()
    {
        if (itemStats.status!=null)
            if (type != SlotType.Weapons)
                stats.RemoveStatus(itemStats.status);
        Vector3 dropOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0.2f);
        Vector3 dropPosition = Camera.main.GetComponent<CameraFollow>().target.position + dropOffset;
        dropItem.GetComponent<Item>().stats = itemStats;
        image.sprite = _imageEmpty;
        StatsChangeEvent.Invoke(itemStats.type, -itemStats.additiveValue);
        itemStats = null;
        Instantiate(dropItem, dropPosition, new Quaternion(0, 0, 0, 0));
    }
}
