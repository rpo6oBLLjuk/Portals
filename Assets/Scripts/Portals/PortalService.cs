using UnityEngine;
using Zenject;

public class PortalService : MonoBehaviour
{
    [Inject] private CameraService cameraService;

    [field: SerializeField] public PortalsGroup Portals { get; private set; }


    private void Start()
    {
        Portals.Start();
    }

    private void Update()
    {
        Portals.Update(cameraService.MainCamera.transform);
    }

    public void SpawnFirstPortal(RaycastHit hit)
    {
        Portals.FirstPortalData.PlacePortal(hit);
    }

    public void SpawnSecondPortal(RaycastHit hit)
    {
        Portals.SecondPortalData.PlacePortal(hit);
    }
}
