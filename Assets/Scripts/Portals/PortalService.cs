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
}
