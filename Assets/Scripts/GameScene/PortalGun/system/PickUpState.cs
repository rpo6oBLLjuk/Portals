using CustomInspector;
using System;
using UnityEngine;
using Zenject;

[Serializable]
public class PickUpState : PortalGunState
{
    [Inject] readonly EffectService effectService;
    [Inject] readonly UIService uiService;

    [SerializeField] private Transform LightningPoint;

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
        effectService.EnablePortalGunLightning();
    }
    public override void DeactivateState()
    {
        base.DeactivateState();
        uiService.CrosshairController.DisableCrosshair();
        effectService.DisablePortalGunLightning();
    }

    public override void StateUpdateLogic()
    {
        GetInput();

        if (PickedUp)
        {
            MoveObj();
            effectService.SetPortalGunLightningPoints(LightningPoint.position, pickedUpObj.transform.position);
        }
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
            //При выключении коллизии "механизмы" ломаются
            //Без выключения коллизии объект бесконечно перемещается между порталами

            //collider.enabled = isActive;
        }
    }
}
