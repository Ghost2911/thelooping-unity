using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class Slot : MonoBehaviour
{
    public ItemStats itemStats;
    public SlotType type;
    public UnityEvent<StatsType,int> StatsChangeEvent;
    public EntityStats stats;

    private GameObject _item;
    private Image _image;
    private Sprite _imageEmpty;

    public void Start()
    {
        _item = Resources.Load("item") as GameObject;
        _image = transform.GetChild(0).GetComponent<Image>();
        _imageEmpty = _image.sprite;
        if (_item == null)
            Debug.Log("Create prefab with name \"item\" in resources folder");
    }

    public void Equip(ItemStats equipItem)
    {
        if (itemStats != null)
            Unequip();

        itemStats = equipItem;
        _image.sprite = itemStats.icon;
        StatsChangeEvent.Invoke(itemStats.type, itemStats.additiveValue);
        if (equipItem.status!=null)
            if (type!=SlotType.Weapons)
                stats.AddStatus(equipItem.status);
    }

    public void Unequip()
    {
        if (itemStats.status!=null)
            if (type != SlotType.Weapons)
                stats.RemoveStatus(itemStats.status);
        Vector3 dropOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0.2f);
        Vector3 dropPosition = Camera.main.GetComponent<CameraFollow>().target.position + dropOffset;
        _item.GetComponent<Item>().stats = itemStats;
        _image.sprite = _imageEmpty;
        StatsChangeEvent.Invoke(itemStats.type, -itemStats.additiveValue);
        itemStats = null;
        Instantiate(_item, dropPosition, new Quaternion(0, 0, 0, 0));
    }
}
