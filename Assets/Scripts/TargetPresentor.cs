using UnityEngine;
using UnityEngine.UI;

public class TargetPresentor : MonoBehaviour
{
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetCompleted()
    {
        _image.color = Color.black;
    }
}
