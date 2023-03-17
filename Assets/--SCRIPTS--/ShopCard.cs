using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    public TextMeshProUGUI tmDescription;
    public Image img;
    public TextMeshProUGUI tmCostValue;
    public Image iconRes;

    public void SetCardInfo(string desc, Sprite image, int costValue, Sprite iconResource)
    {
        tmDescription.text=desc;
        img.sprite=image;
        tmCostValue.text=costValue.ToString();
        iconRes.sprite=iconResource;
    }
}
