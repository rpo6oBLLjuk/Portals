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


    private void Start()
    {
        secondPortal = (isFirst) ? portalService.Portals.SecondPortalData : portalService.Portals.FirstPortalData;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Enter, GO: {other.gameObject.name}");

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
        player.transform.position = secondPortal.PortalContainer.position;

        player.transform.forward += -portalService.Portals.SecondPortalData.PortalContainer.forward;


        Debug.Log($"Teleport ended", this);
    }
}
