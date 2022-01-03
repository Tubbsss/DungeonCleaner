using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    public Texture2D crosshairs;

    private void Awake()
    {
        instance = this;
    }

    public void ActivateCrosshairs() //Set cursor to crosshair.
    {
        Cursor.SetCursor(crosshairs, new Vector2(crosshairs.width/2, crosshairs.height/2), CursorMode.Auto);
    }

    public void ClearCursor() //Reset cursor.
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
