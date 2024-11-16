using CustomInspector;
using UnityEngine;
using Zenject;

public class PortalGunController : MonoBehaviour
{
    [Inject] UIService uiService;

    [SerializeField, Tab("Raycast")] private LayerMask raycastMask;
    [SerializeField, Tab("Raycast")] private float raycastDistance;


    [SerializeField, Tab("Debug"), ReadOnly] private Vector3? hitPoint = new(0, 0, 0);
    [SerializeField, Tab("Debug")] private Color hitPointColor = Color.red;
    [SerializeField, Tab("Debug")] private float hitPointRadius = 0.1f;


    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward * raycastDistance, out RaycastHit hit, raycastDistance, raycastMask))
        {
            uiService.CrosshairController.EnableCrosshair();
            hitPoint = hit.point;
        }
        else
        {
            uiService.CrosshairController.DisableCrosshair();
            hitPoint = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);

        if (hitPoint != null)
        {
            Gizmos.color = hitPointColor;
            Gizmos.DrawWireSphere(hitPoint.Value, hitPointRadius);
        }
    }
}
