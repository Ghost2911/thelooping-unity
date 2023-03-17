using UnityEngine;
using TMPro;

public class Item : MonoBehaviour, IUsable
{
    public ItemStats stats;
    public bool inMarket = false;
    public bool isShow = false;

    private Animator _animator;
    private TextMeshPro _textMesh;
    private PlayerInput _player;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = stats.icon;
        _animator = GetComponentInChildren<Animator>();
        _textMesh = GetComponentInChildren<TextMeshPro>();
        if (stats.slotType == SlotType.Potions)
        {
            _textMesh.text = $"???\n{stats.costPrice.ToString().PadLeft(2, '0')} <sprite name=\"icon_res_{stats.costItem.type.ToString()}\">";
        }
        else
        {
            _textMesh.text = $"{stats.additiveValue.ToString().PadLeft(2,'0')} <sprite name=\"icon_res_{stats.type.ToString()}\">\n{stats.costPrice.ToString().PadLeft(2, '0')} <sprite name=\"icon_res_{stats.costItem.type.ToString()}\">";
        }
    }

    public void Use(EntityStats entityStats)
    {
        if (isShow)
        {
            if (inMarket)
            {
                if (_player.inventory.ChangeCollectableItem(stats.costItem.type, -stats.costPrice))
                {
                    _player.inventory.Equip(stats);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("�� ������� �������� ��� �������");
                }
            }
            else
            {
                _player.inventory.Equip(stats);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _player = other.gameObject.GetComponent<PlayerInput>();
            _animator.SetTrigger("ShowText");
            isShow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _player = null;
            _animator.SetTrigger("HideText");
            isShow = false;
        }
    }
}
