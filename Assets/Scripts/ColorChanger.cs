using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Texture2D characterTexture2D;
    public Color replaceColor;
    public Color updateColor;
    public bool swapColor = false;

    public Texture2D CopyTexture2D(Texture2D copiedTexture)
    {
        //Create a new Texture2D, which will be the copy.
        Texture2D texture = new Texture2D(copiedTexture.width, copiedTexture.height);
        //Choose your filtermode and wrapmode here.
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        int y = 0;
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {
                //INSERT YOUR LOGIC HERE
                if (copiedTexture.GetPixel(x, y) == replaceColor)
                {
                    //This line of code and if statement, turn Green pixels into Red pixels.
                    texture.SetPixel(x, y, updateColor);
                }
                else
                {
                    //This line of code is REQUIRED. Do NOT delete it. This is what copies the image as it was, without any change.
                    texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
                }
                ++x;
            }
            ++y;
        }
        texture.Apply();

        //Return the variable, so you have it to assign to a permanent variable and so you can use it.
        return texture;
    }

    public void UpdateCharacterTexture()
    {
        //This calls the copy texture function, and copies it. The variable characterTextures2D is a Texture2D which is now the returned newly copied Texture2D.
        characterTexture2D = CopyTexture2D(gameObject.GetComponent<SpriteRenderer>().sprite.texture);

        //Get your SpriteRenderer, get the name of the old sprite,  create a new sprite, name the sprite the old name, and then update the material. If you have multiple sprites, you will want to do this in a loop- which I will post later in another post.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        string tempName = sr.sprite.name;
        sr.sprite = Sprite.Create(characterTexture2D, sr.sprite.rect, new Vector2(0, 1));
        sr.sprite.name = tempName;

        sr.material.mainTexture = characterTexture2D;
    }


    private void Update()
    {
        if (swapColor)
        {
            UpdateCharacterTexture();
            swapColor = false;
        }
    }
}
