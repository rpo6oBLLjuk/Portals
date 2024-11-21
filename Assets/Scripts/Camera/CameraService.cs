using Cinemachine;
using UnityEngine;

public class CameraService : MonoBehaviour
{
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera CameraTransform { get; private set; }

    [SerializeField] private bool cursorVisibility = false;

    public Quaternion Rotation
    {
        get => MainCamera.transform.rotation;
    }

    public void Start()
    {
        Cursor.visible = cursorVisibility;
        Cursor.lockState = CursorLockMode.Locked;

        QualitySettings.vSyncCount = 0;
    }

}
