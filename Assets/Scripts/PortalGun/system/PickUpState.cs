using CustomInspector;
using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PickUpState : PortalGunState
{
    [Inject] UIService uiService;

    [field: SerializeField, ReadOnly] public bool PickedUp { get; private set; }

    [SerializeField] private KeyCode pickupKeyCode;
    [SerializeField] private float objDistance = 1;

    [SerializeField] private float throwForce = 10;

    [SerializeField, Range(0, 1)] private float lerpPositionValue;

    [SerializeField] private GameObject pickedUpObj;


    public override void ActivateState()
    {
        base.ActivateState();
        uiService.CrosshairController.EnablePickupCrosshair();
    }
    public override void DeactivateState()
    {
        base.DeactivateState();
        uiService.CrosshairController.DisableCrosshair();
    }

    public override void StateUpdateLogic()
    {
        GetInput();

        if (PickedUp)
            MoveObj();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(pickupKeyCode))
        {
            if (PickedUp)
                PutObj();
            else
                PickUpObj();
        }

        if (PickedUp)
        {
            if (Input.GetMouseButtonDown(0))
                ThrowObj();

            if (Input.GetMouseButtonDown(1))
                PutObj();
        }
    }

    private void MoveObj()
    {
        Vector3 focusPosition = cameraService.MainCamera.transform.position + cameraService.MainCamera.transform.forward * objDistance;
        pickedUpObj.transform.position = Vector3.Lerp(pickedUpObj.transform.position, focusPosition, lerpPositionValue);
    }

    private void PickUpObj()
    {
        PickedUp = true;
        pickedUpObj = raycastHit.collider.gameObject;

        SetObjPhysicsActive(false);
    }

    private void PutObj()
    {
        PickedUp = false;
        SetObjPhysicsActive(true);
    }
    private void ThrowObj()
    {
        PickedUp = false;
        SetObjPhysicsActive(true);

        if (pickedUpObj.TryGetComponent(out Rigidbody rb))
            rb.AddForce(cameraService.MainCamera.transform.forward * throwForce);
    }

    private void SetObjPhysicsActive(bool isActive)
    {
        if (pickedUpObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = !isActive;
        }
        if(pickedUpObj.TryGetComponent(out Collider collider))
        {
            collider.enabled = isActive;
        }
    }
}
