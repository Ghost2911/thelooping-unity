using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [Header("Main info")]
    public SlotType slotType;
    public Sprite icon;
    [Header("Item stats")]
    public StatsType stats;
    public int additiveValue;
    [Header("Item cost")]
    public CollectableItem costItem;
    public int costPrice;

   
    public bool inMarket = false;
    public bool isShow = false;

    private Animator _animator;
    private TextMeshPro _textMesh;

    void Start()
    {
        Transform childText = transform.GetChild(0);
        _animator = childText.GetComponent<Animator>();
        _textMesh = childText.GetComponent<TextMeshPro>();
        _textMesh.text = string.Format("{0} <sprite name=\"icon_collect_{1}\">\n\n{2} <sprite name=\"icon_collect_{3}\">",
            additiveValue, stats.ToString(), costPrice, costItem.type.ToString());
        GetComponent<SpriteRenderer>().sprite = icon;
        
    }

    private void OnMouseUp()
    {
        Debug.Log("Click - text");
        if (isShow)
        {
            Slot[] slots = PlayerInput.instance.inventory.slots;
            foreach (Slot slot in slots)
            {
                if (slot.type == slotType)
                {
                    slot.Equip(this);
                    break;
                }
            }
            Destroy(gameObject); 
        }
        else
        { }
    }
    private void OnTriggerEnter(Collider other)
    {
        _animator.SetTrigger("ShowText");
        isShow = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetTrigger("HideText");
        isShow = false;
    }
}
