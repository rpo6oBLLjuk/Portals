using CustomInspector;
using System;
using UnityEngine;
using Zenject;

[Serializable]
public abstract class PortalGunState
{
    [Inject] protected PlayerService playerService;
    [Inject] protected CameraService cameraService;

    public PortalGunStateController portalGunStateController;

    [SerializeField, Layer] protected int surfaceMask;
    [SerializeField] protected LayerMask raycastMask;
    [SerializeField] protected float raycastDistance;

    protected RaycastHit raycastHit;
    protected bool activeState = false;


    public void Update()
    {
        StateUpdateLogic();
    }

    public bool CheckRaycast()
    {
        if (Physics.Raycast(cameraService.MainCamera.transform.position, cameraService.MainCamera.transform.forward, out raycastHit, raycastDistance, raycastMask))
        {
            if (raycastHit.collider.gameObject.layer == surfaceMask && raycastHit.collider.gameObject != playerService.Player)
            {
                if (!activeState)
                    ActivateState();
                return true;
            }
        }

        if (activeState)
            DeactivateState();
        return false;
    }

    public abstract void StateUpdateLogic();

    public virtual void ActivateState()
    {
        activeState = true;
    }
    public virtual void DeactivateState()
    {
        activeState = false;
    }
}
