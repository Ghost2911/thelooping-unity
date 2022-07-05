using UnityEngine;
using UnityEngine.UI;

public class HealthPresentor : MonoBehaviour
{
    public Image[] hpRenderer;
    public Sprite fullHp;
    public Sprite halfHp;
    public Sprite emptyHp;

    public void ChangeValue(int value)
    {
        int fullHeart = value / 2, halfHeart = value % 2;
        for (int i = 0; i < fullHeart; i++)
            hpRenderer[i].sprite = fullHp;
        if (halfHeart != 0)
            hpRenderer[fullHeart++].sprite = halfHp;
        for (int i = fullHeart; i < hpRenderer.Length; i++)
            hpRenderer[i].sprite = emptyHp;

    }
}
