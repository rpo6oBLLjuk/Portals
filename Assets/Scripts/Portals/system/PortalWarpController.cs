using CustomInspector;
using UnityEngine;
using Zenject;

public class PortalWarpController : MonoBehaviour
{
    [Inject] PortalService portalService;
    [Inject] PlayerService playerService;
    [Inject] CameraService cameraService;

    [SerializeField] private bool isFirst = true;
    [SerializeField, Layer] private int warpedObjectsLayer;

    [SerializeField] private bool isActive = true;
    [SerializeField] private bool breakBeforeWarp = false;

    [SerializeField, ReadOnly] private PortalData secondPortal;

    private ListContainer<Collider> colliders = new ListContainer<Collider>();


    private void Start()
    {
        secondPortal = (isFirst) ? portalService.Portals.SecondPortalData : portalService.Portals.FirstPortalData;
    }



    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);

        if (!isActive)
            return;

        if (other.gameObject.layer == warpedObjectsLayer)
        {
            colliders.Remove(other);

            Debug.Log($"Trigger Enter, GO: {other.gameObject.name}", other.gameObject);

            if (true)
            {
                Warp(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);

        if (other.gameObject.layer == warpedObjectsLayer)
        {
            IngoneCollision(other, false);
        }
    }

    public bool CheckPlayerDirection()
    {
        return false;
    }

    private void Warp(Collider warpedObj)
    {
        IngoneCollision(warpedObj, true);
        secondPortal.WarpController.IngoneCollision(warpedObj, true);

        Quaternion halfTurn = portalService.Portals.GetPortalRotationDifference(transform, secondPortal);

        var inTransform = this.transform;
        var outTransform = secondPortal.Container.transform;

        Vector3 relativePos = inTransform.InverseTransformPoint(warpedObj.transform.position);
        relativePos = halfTurn * relativePos;
        warpedObj.transform.position = outTransform.TransformPoint(relativePos);

        Quaternion relativeRot = warpedObj.transform.rotation;
        relativeRot = halfTurn * relativeRot;
        warpedObj.transform.rotation = outTransform.rotation * relativeRot;

        //Vector3 relativeVel = inTransform.InverseTransformDirection(playerService.Velocity);
        //relativeVel = halfTurn * relativeVel;
        //playerService.Velocity = outTransform.TransformDirection(relativeVel);

        playerService.BreakPlayerMoverUpdate();

        Debug.Log($"Position: {outTransform.TransformPoint(relativePos)}, Rotation: {outTransform.rotation * relativeRot}, HalfTurn: {halfTurn}");

        if (breakBeforeWarp)
            Debug.Break();
    }

    public void IngoneCollision(Collider warpedObj, bool ignore)
    {
        string debugString = $"Obj to {ignore} ignore: {warpedObj.name} | Ignored: ";

        Collider warpedCollider = warpedObj.GetComponent<Collider>();

        foreach (Collider otherColliders in colliders)
        {
            Physics.IgnoreCollision(warpedCollider, otherColliders, ignore);
            debugString += $"{otherColliders.name}, ";
        }

        Debug.Log(debugString, this);
    }
}
