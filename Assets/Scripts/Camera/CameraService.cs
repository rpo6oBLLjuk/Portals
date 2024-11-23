using UnityEngine;

public class CameraService : MonoBehaviour
{
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public Transform CameraTransform { get; private set; }

    [SerializeField] private bool cursorVisibility = false;


    public void Start()
    {
        Cursor.visible = cursorVisibility;
        Cursor.lockState = CursorLockMode.Locked;

        QualitySettings.vSyncCount = 0;
    }

    public void SetCameraLocalPosition(Vector3 localPosition)
    {
        CameraTransform.localPosition = localPosition;
    }
}
