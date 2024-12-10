using CustomInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CrosshairController
{
    [SerializeField] private Image crosshair;

    [SerializeField, Preview(Size.medium)] private Sprite portal;
    [SerializeField, Preview(Size.medium)] private Sprite pickedObj;
    [SerializeField, Preview(Size.medium)] private Sprite inactive;


    public void EnablePortalCrosshair()
    {
        crosshair.sprite = portal;
    }

    public void EnablePickupCrosshair()
    {
        crosshair.sprite = pickedObj;
    }

    public void DisableCrosshair()
    {
        crosshair.sprite = inactive;
    }
}
