using CustomInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CrosshairController
{
    [SerializeField] private Image crosshair;

    [SerializeField, Preview(Size.medium)] private Sprite active;
    [SerializeField, Preview(Size.medium)] private Sprite inactive;

    public void EnableCrosshair()
    {
        crosshair.sprite = active;
    }

    public void DisableCrosshair()
    {
        crosshair.sprite = inactive;
    }
}
