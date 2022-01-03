using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//===============================================================================================================
// Adapted from script by Code Monkey - https://unitycodemonkey.com/video.php?v=Xss4__kgYiY
//===============================================================================================================

public class CleanableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Texture2D dirtMaskTextureBase;
    [SerializeField]
    private Texture2D dirtBrushBase;
    [SerializeField]
    private Texture2D dirtBrush;
    [SerializeField]
    private Material material;
    private Texture2D dirtMaskTexture;
    public float dirtAmountTotal;
    public float dirtAmount;
    public float dirtThreshold;
    private Vector2Int lastPaintPixelPosition;
    private int initialBrushWidth;
    private int initialBrushHeight;
    private int layerMask = 1 << 8;

    private void Awake()
    {
        //Get brush size from Brush Base and set dirtBrush size.
        dirtBrush = new Texture2D(dirtBrushBase.width, dirtBrushBase.height);
        dirtBrush.SetPixels(dirtBrushBase.GetPixels());
        dirtBrush.Apply();

        //Store brush size.
        initialBrushWidth = dirtBrush.width;
        initialBrushHeight = dirtBrush.height;

        //Get dirt mask texture size and pixels.
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_DirtMask", dirtMaskTexture); //Apply dirt mask texture to material.

        dirtAmountTotal = 0f; //Reset dirt amount total.
        for (int x = 0; x < dirtMaskTextureBase.width; x++)
        {
            for (int y = 0; y < dirtMaskTextureBase.height; y++)
            {
                dirtAmountTotal += dirtMaskTextureBase.GetPixel(x, y).g; //Calculate dirt amount total.
            }
        }
        dirtAmount = dirtAmountTotal; //Set dirt amount to dirt amount total.
    }

    // Update is called once per frame
    void Update()
    {
        SetBrushSize(); //Set brush size.

        RaycastHit raycastHit;

            if (Physics.Raycast(player.transform.position, Vector3.down, out raycastHit, 10.0f, layerMask)) //If player on object.
            {
                Vector2 textureCoord = raycastHit.textureCoord; //Get texture coordinate.

                int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY); //Set paintbrush position.

                int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                int maxPaintDistance = 7;
                if (paintPixelDistance < maxPaintDistance)
                {
                    // Painting too close to last position
                    return;
                }
                lastPaintPixelPosition = paintPixelPosition;
                
                //Centre brush on player.
                int pixelXOffset = pixelX - (dirtBrush.width / 2);
                int pixelYOffset = pixelY - (dirtBrush.height / 2);

                for (int x = 0; x < dirtBrush.width; x++)
                {
                    for (int y = 0; y < dirtBrush.height; y++)
                    {
                        Color pixelDirt = dirtBrush.GetPixel(x, y);
                        Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                        dirtAmount -= removedAmount; //Remove amount painted from dirt amount.

                        dirtMaskTexture.SetPixel(
                            pixelXOffset + x,
                            pixelYOffset + y,
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                        );
                    }
                }

                dirtMaskTexture.Apply();
        }
    }

    public float GetDirtAmount()
    {
        return this.dirtAmount / dirtAmountTotal;
    }

    private void SetBrushSize()
    {
        //Multiply brush size by player size.
        int brushWidth = (int)(initialBrushWidth * player.GetComponent<PlayerController>().sizeMultiplier);
        int brushHeight = (int)(initialBrushHeight * player.GetComponent<PlayerController>().sizeMultiplier);

        dirtBrush.Resize(brushWidth, brushHeight); //Resize brush to new size.

        //Fill brush with black.
        Color32 fillColor = new Color32(0, 0, 0, 0);
        Color32[] fillColorArray = dirtBrush.GetPixels32();

        for (int i = 0; i < fillColorArray.Length; i++)
        {
            fillColorArray[i] = fillColor;
        }

        dirtBrush.SetPixels32(fillColorArray);

        dirtBrush.Apply();
    }
}
