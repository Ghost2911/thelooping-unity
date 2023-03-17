using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Material material;
    public Color[] paintColors = new Color[2];

    public void SwapColor(SlotType slotType, Color color)
    {
        if (material == null)
            material = GetComponent<SpriteRenderer>().material;

        if (color != Color.black)
        {
            material.SetColor($"_ColorIn{slotType}", paintColors[(int)slotType]);
            material.SetColor($"_ColorOut{slotType}", color);
        }
        else
        {
            material.SetColor($"_ColorIn{slotType}", new Color(0,0,0,0));
        }
    }
}

