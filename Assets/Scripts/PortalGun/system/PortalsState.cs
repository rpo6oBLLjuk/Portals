using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PortalsState : PortalGunState
{
    [Inject] PortalService portalService;
    [Inject] UIService uiService;


    public override void ActivateState()
    {
        base.ActivateState();
        uiService.CrosshairController.EnablePortalCrosshair();
    }

    public override void DeactivateState()
    {
        base.DeactivateState();
        uiService.CrosshairController.DisableCrosshair();
    }

    public override void StateUpdateLogic()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            portalService.SpawnFirstPortal(raycastHit);
        }
        if (Input.GetMouseButtonDown(1))
        {
            portalService.SpawnSecondPortal(raycastHit);
        }
    }
}
