using CustomInspector;
using UnityEditor;
using UnityEngine;
using Zenject;

[CanEditMultipleObjects]
public class PortalTeleportController : MonoBehaviour
{
    [Inject] PortalService portalService;
    [Inject] PlayerService playerService;
    [Inject] CameraService cameraService;

    [SerializeField] private bool isFirst = true;
    [SerializeField, Layer] private int playerLayer;

    [SerializeField, ReadOnly] private PortalData secondPortal;

    [SerializeField] private bool isActive = true;


    private void Start()
    {
        secondPortal = (isFirst) ? portalService.Portals.SecondPortalData : portalService.Portals.FirstPortalData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
            return;

        Debug.Log($"Trigger Enter, GO: {other.gameObject.name}", other.gameObject);

        if (other.gameObject.layer == playerLayer)
        {
            if (true)
            {
                Teleport(other.gameObject);
            }
        }
    }

    public bool CheckPlayerDirection()
    {
        if (playerService.Velocity.magnitude > 1)
            return true;

        return false;
    }

    private void Teleport(GameObject player)
    {
        player.transform.position = Vector3.zero;

        //player.transform.rotation = portalService.Portals.SecondPortalData.PortalContainer.rotation;
        Debug.Break();

        Debug.Log($"Teleport ended, Position: {secondPortal.PortalContainer.position}", this);
    }
}
