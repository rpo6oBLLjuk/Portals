using UnityEngine;
using Zenject;

public class PlayerBodyRotator : MonoBehaviour
{
    [Inject] CameraService cameraService;

    [SerializeField] private Transform body;
    public Quaternion rotation { get; private set; }

    private void Update()
    {
        Quaternion cameraRotation = cameraService.MainCamera.transform.rotation;
        cameraRotation.x = 0;
        cameraRotation.z = 0;

        rotation = cameraRotation;
        body.rotation = cameraRotation;
    }
}
