using CustomInspector;
using UnityEngine;
using Zenject;

public class PortalWarpController : MonoBehaviour
{
    [Inject] readonly CameraService cameraService;
    [Inject] readonly PortalService portalService;
    [Inject] readonly AudioService audioService;
    [Inject] readonly UIService uiService;

    [SerializeField] private bool isFirst = true;
    [SerializeField, Layer] private int warpedObjectsLayer;

    [SerializeField] private bool breakBeforeWarp = false;

    [SerializeField] private bool collisionLog = false;

    [SerializeField, ReadOnly] private PortalData secondPortal;

    [SerializeField]private ListContainer<Collider> colliders = new ();


    private void Start()
    {
        secondPortal = (isFirst) ? portalService.Portals.SecondPortalData : portalService.Portals.FirstPortalData;
    }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);

        if (collisionLog)
            Debug.Log($"Trigger Enter, GO: {other.gameObject.name}", other.gameObject);

        if (other.gameObject.layer == warpedObjectsLayer)
        {
            colliders.Remove(other);

            IngoneCollision(other, true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == warpedObjectsLayer)
        {
            if (CheckPlayerPosition(other))
                Warp(other);

            IngoneCollision(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(collisionLog)
            Debug.Log($"Trigger Exit, GO: {other.gameObject.name}", other.gameObject);

        if (other.gameObject.layer == warpedObjectsLayer)
        {
            IngoneCollision(other, false);
        }
        else
        {
            colliders.Remove(other);
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

        var inTransform = this.transform;
        var outTransform = secondPortal.Container.transform;

        Vector3 relativePos = inTransform.worldToLocalMatrix.MultiplyPoint3x4(warpedObj.transform.position);
        relativePos.x *= -1;
        relativePos.z *= -1;
        relativePos = outTransform.localToWorldMatrix.MultiplyPoint3x4(relativePos);
        warpedObj.transform.position = relativePos;

        Quaternion relativeRot = outTransform.transform.rotation * Quaternion.Inverse(inTransform.rotation * Quaternion.Euler(0, 180, 0));
        warpedObj.transform.rotation *= relativeRot;

        Vector3 relativeVel;


        if (warpedObj.TryGetComponent(out PlayerPhysicsController entityPhysicsController))
        {
            Vector3 force = entityPhysicsController.Velocity - entityPhysicsController.MovementVelocity;

            // Преобразование Vector3 в кватернион (чистый кватернион)
            Quaternion forceQuaternion = new Quaternion(force.x, force.y, force.z, 0f);
            // Вращение кватерниона
            Quaternion rotatedForceQuaternion = relativeRot * forceQuaternion * Quaternion.Inverse(relativeRot);
            // Преобразование обратно в Vector3
            relativeVel = new Vector3(rotatedForceQuaternion.x, rotatedForceQuaternion.y, rotatedForceQuaternion.z);
            // Применение силы
            //entityPhysicsController.ChangeForce(relativeVel);

            Debug.DrawRay(relativePos, relativeVel.normalized * 10);

            uiService.ConsoleWidget.AddLog($"Teleport end\nEntity: <color=green>{warpedObj.name}</color>\nPos: {relativePos}\nRot: {relativeRot}\nVel: {relativeVel}");
        }
        else if (warpedObj.TryGetComponent(out Rigidbody rb))
        {
            Vector3 force = rb.velocity;

            Quaternion forceQuaternion = new Quaternion(force.x, force.y, force.z, 0f);
            // Вращение кватерниона
            Quaternion rotatedForceQuaternion = relativeRot * forceQuaternion * Quaternion.Inverse(relativeRot);
            // Преобразование обратно в Vector3
            relativeVel = new Vector3(rotatedForceQuaternion.x, rotatedForceQuaternion.y, rotatedForceQuaternion.z);
            rb.velocity = relativeVel;

            Debug.DrawRay(relativePos, relativeVel.normalized * 10);
            uiService.ConsoleWidget.AddLog($"Teleport end\nEntity: <color=red>{warpedObj.name}</color>\nPos: {relativePos}\nRot: {relativeRot}\nVel: {relativeVel}");
        }

        if (warpedObj.TryGetComponent(out PlayerRotationFix playerRotationFix))
        {
            playerRotationFix.FixRotation();
        }

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

        if(warpedObj.TryGetComponent(out PlayerPhysicsController playerPhysicsController))
        {
            playerPhysicsController.IngoreCollision(colliders, ignore);
        }

        if (collisionLog)
            Debug.Log(debugString, this);
    }
}
