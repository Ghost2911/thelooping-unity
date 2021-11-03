using UnityEngine;
using UnityEngine.UI;

public class TargetPresentor : MonoBehaviour
{
    private Image _image;
    private string _targetName;
    private void Start()
    {
        _image = GetComponent<Image>();
    }
    public void SetTarget(Sprite sprite, string name)
    {
        _image.sprite = sprite;
        _targetName = name;
    }
}
