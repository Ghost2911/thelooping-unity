using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
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
        _textMesh.text = string.Format("{0} <sprite name=\"icon_res_{1}\">\n{2} <sprite name=\"icon_res_{3}\">",
            stats.additiveValue.ToString().PadLeft(2,'0'),  stats.type.ToString(), stats.costPrice.ToString().PadLeft(2, '0'), stats.costItem.type.ToString());
    }

    private void OnMouseUp()
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
                    //отмена покупки - анимация нехватки ресурса
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
