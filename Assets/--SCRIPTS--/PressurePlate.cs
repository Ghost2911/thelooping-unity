using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    public bool isActive = false;
    public bool isCorrect = false;

    private void OnTriggerEnter(Collider other) {
        if (other.tag=="Player")
        {
            if (!isActive)
            {
                isActive = true;
                GetComponent<SpriteRenderer>().sprite = activeSprite;
                if (!isCorrect)
                    transform.parent.GetComponent<PuzzlePlate>().DeactivateAllPlates();
            }
        }
    }

    public void Deactivate()
    {
        isActive = false;
        GetComponent<SpriteRenderer>().sprite = inactiveSprite;
    }
}
