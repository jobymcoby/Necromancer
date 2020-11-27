using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolorPixel : MonoBehaviour
{
    [SerializeField] private Sprite dead;
    [SerializeField] private Sprite alive;
    private SpriteRenderer rend;
    private Texture2D live;
    private Texture2D deader;
    private Color[] liveColors;
    private Color[] deadColors;
    private bool fullDead = false;
    private bool[] currentMix = new bool[16 * 16];

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        Texture2D tileTexture = new Texture2D(16, 16)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };

        // Find the position of the tile. 
        Rect tilePos = alive.rect;

        // Choose a random tile of the same type
        int randomTile = UnityEngine.Random.Range(0, 3) * 64 + (int)alive.rect.xMin;

        // Get the pixels from the texture from the sprites location
        liveColors = alive.texture.GetPixels(
            randomTile, 
            (int)alive.rect.yMin, 
            (int)alive.rect.height, 
            (int)alive.rect.width
        );

        tileTexture.SetPixels(liveColors);
        tileTexture.Apply();
        rend.sprite = Sprite.Create(
            tileTexture, 
            new Rect(0, 0, 16, 16), 
            Vector2.one * .5f,
            16
        );

        deader = dead.texture;
        deadColors = deader.GetPixels(
            randomTile,
            (int)alive.rect.yMin,
            (int)alive.rect.height,
            (int)alive.rect.width
        );    
    }

    public void MixDeadGrass(bool[] mixKey)
    {
        if (fullDead)
            return;

        Sprite mixSprite = Sprite.Create(MixTextures(mixKey), new Rect(0, 0, 16, 16), Vector2.one * .5f, 16);

        rend.sprite = mixSprite;
    }

    private Texture2D MixTextures (bool[] mixKey)
    {
        int counter = 0;
        Texture2D mix = new Texture2D(16, 16)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };

        Color[] colorArray = new Color[mix.height * mix.width];

        //Goes through each pixel
        for (int x = 0; x < mix.width; x++)
        {
            for (int y = 0; y < mix.height; y++)
            {
                int arrayIndex = x + (y * mix.height);
                if (mixKey[arrayIndex])
                {
                    // Set pixel to dead color
                    colorArray[arrayIndex] = deadColors[arrayIndex];
                    currentMix[arrayIndex] = true;
                    counter += 1;
                }
                else if (currentMix[arrayIndex])
                {
                    colorArray[arrayIndex] = deadColors[arrayIndex];
                }
                else
                    colorArray[arrayIndex] = liveColors[arrayIndex];
            }
        }

        //if (counter > 64)  // all the pixels are dead
        //{
        //    fullDead = true;
        //}

        mix.SetPixels(colorArray);
        mix.Apply();

        return mix;
    }
}
