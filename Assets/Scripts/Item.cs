using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemStats stats;
    public bool inMarket = false;
    public bool isShow = false;

    private Animator _animator;
    private TextMeshPro _textMesh;
    private GameObject _player;

    void Start()
    {
        Transform childText = transform.GetChild(0);
        _animator = childText.GetComponent<Animator>();
        _textMesh = childText.GetComponent<TextMeshPro>();
        _textMesh.text = string.Format("{0} <sprite name=\"icon_collect_{1}\">\n\n{2} <sprite name=\"icon_collect_{3}\">",
            stats.additiveValue, stats.type.ToString(), stats.costPrice, stats.costItem.type.ToString());
        GetComponent<SpriteRenderer>().sprite = stats.icon;
    }

    private void OnMouseUp()
    {
        if (isShow)
        {
            _player.GetComponent<PlayerInput>().inventory.Equip(stats);
            Destroy(gameObject); 
        }
        else
        { }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _player = other.gameObject;
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
