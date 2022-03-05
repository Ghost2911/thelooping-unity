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
        Transform childText = transform.GetChild(0);
        _animator = childText.GetComponent<Animator>();
        _textMesh = childText.GetComponent<TextMeshPro>();
        _textMesh.text = string.Format("{0} <sprite name=\"resources_basic_0\">\n\n{2} <sprite name=\"resources_basic_0\">",
            stats.additiveValue, stats.type.ToString(), stats.costPrice, stats.costItem.type.ToString());
        GetComponent<SpriteRenderer>().sprite = stats.icon;
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
