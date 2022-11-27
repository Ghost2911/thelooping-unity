using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject linkedDoor;
    public Sprite openedDoor;
    public Sprite closedDoor;
    public bool isOpen = true;

    private SpriteRenderer _renderer;
    private Collider _collider;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("stay - "+other.name);
        if (other.CompareTag("Player"))
        {   
            other.transform.position = linkedDoor.transform.position - linkedDoor.transform.forward*4;
        }
    }

    public void ChangeState()
    {
        if (isOpen)
        {
            _collider.enabled = false;
            _renderer.sprite = closedDoor;
        }
        else
        {
            _collider.enabled = true;
            _renderer.sprite = openedDoor;
        }

    }
}
