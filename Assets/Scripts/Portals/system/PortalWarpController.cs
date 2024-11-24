using CustomInspector;
using UnityEngine;
using Zenject;

public class PortalWarpController : MonoBehaviour
{
    [Inject] readonly CameraService cameraService;
    [Inject] readonly PortalService portalService;
    [Inject] readonly AudioService audioService;

    [SerializeField] private bool isFirst = true;
    [SerializeField, Layer] private int warpedObjectsLayer;

    [SerializeField] private bool breakBeforeWarp = false;

    [SerializeField] private bool collisionLog = false;

    [SerializeField, ReadOnly] private PortalData secondPortal;

    private ListContainer<Collider> colliders = new ListContainer<Collider>();


    private void Start()
    {
        secondPortal = (isFirst) ? portalService.Portals.SecondPortalData : portalService.Portals.FirstPortalData;
    }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);

        Debug.Log($"Trigger Enter, GO: {other.gameObject.name}", other.gameObject);

        if (other.gameObject.layer == warpedObjectsLayer)
        {
            IngoneCollision(other, true);

            colliders.Remove(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == warpedObjectsLayer)
        {
            if (CheckPlayerPosition(other))
                Warp(other);
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

    public bool CheckPlayerPosition(Collider other)
    {
        Vector3 localPosition = transform.InverseTransformPoint(other.gameObject.transform.position);
        if (localPosition.z > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void Warp(Collider warpedObj)
    {
        secondPortal.WarpController.IngoneCollision(warpedObj, true);

        audioService.Warp();

        Quaternion halfTurn = portalService.Portals.GetPortalRotationDifference(transform, secondPortal);

        var inTransform = this.transform;
        var outTransform = secondPortal.Container.transform;

        Vector3 relativePos = inTransform.InverseTransformPoint(warpedObj.transform.position);
        relativePos = halfTurn * relativePos;
        //relativePos.z *= -1;
        relativePos = outTransform.TransformPoint(relativePos);
        warpedObj.transform.position = relativePos;

        Quaternion relativeRot = warpedObj.transform.rotation;
        relativeRot = halfTurn * relativeRot;
        Quaternion endRotation = relativeRot * outTransform.rotation * Quaternion.Inverse(inTransform.rotation); //Поле для отладки, не забыть удалить
        warpedObj.transform.rotation = endRotation;

        if(warpedObj.TryGetComponent(out Rigidbody rb))
        {
            Vector3 relativeVel = inTransform.InverseTransformDirection(rb.velocity);
            relativeVel = halfTurn * relativeVel;
            rb.velocity = outTransform.TransformDirection(relativeVel);
        }

        if (warpedObj.TryGetComponent(out PlayerRotationFix playerRotationFix))
        {
            playerRotationFix.FixRotation();
        }

        Debug.Log($"EndPosition: {relativePos}, EndRotation: {endRotation}");

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

        if (collisionLog)
            Debug.Log(debugString, this);
    }
}
