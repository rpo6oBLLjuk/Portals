using CustomInspector;
using UnityEngine;
using Zenject;

public class PortalGunController : MonoBehaviour
{
    [Inject] PortalService portalService;
    [Inject] UIService uiService;

    [SerializeField, Tab("Raycast")] private LayerMask raycastMask;
    [SerializeField, Tab("Raycast")] private float raycastDistance;


    [SerializeField, Tab("Debug"), ReadOnly] private RaycastHit raycastHit;
    [SerializeField, Tab("Debug")] private Color hitPointColor = Color.red;
    [SerializeField, Tab("Debug")] private float hitPointRadius = 0.1f;


    void Update()
    {
        if (CheckRaycast())
        {
            GetInput();
        }
    }

    private bool CheckRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, raycastDistance))
        {
            // Проверяем, попадает ли объект в слой маски
            if ((raycastMask & (1 << raycastHit.collider.gameObject.layer)) != 0)
            {
                uiService.CrosshairController.EnableCrosshair();
                return true;
            }
        }

        uiService.CrosshairController.DisableCrosshair();
        return false;
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



    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);

        if (raycastHit.point != null)
        {
            Gizmos.color = hitPointColor;
            Gizmos.DrawWireSphere(raycastHit.point, hitPointRadius);
        }
    }
}
