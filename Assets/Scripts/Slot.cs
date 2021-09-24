using UnityEngine.UI;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Item _item;
    public SlotType type;
    private Image _image;
    private Sprite _imageEmpty;

    public void Start()
    {
        _image = transform.GetChild(0).GetComponent<Image>();
        _imageEmpty = _image.sprite;
    }

    public void Equip(Item item)
    {
        if (_item != null)
        {
            Unequip();
        }
        _item = item;
        _image.sprite = item.icon;
    }

    public void Unequip()
    {
        //drop gameobject
        _item = null;
        _image.sprite = _imageEmpty;
    }
}
