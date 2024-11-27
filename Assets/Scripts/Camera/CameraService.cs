using UnityEngine;

public class CameraService : MonoBehaviour
{
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public Transform CameraTransform { get; private set; }

    [SerializeField] private bool cursorVisibility = false;


    public void Start()
    {
        SetCursorState(cursorVisibility);

        QualitySettings.vSyncCount = 0;
    }

    public void SetCameraLocalPosition(Vector3 localPosition)
    {
        CameraTransform.localPosition = localPosition;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SetCursorState(!cursorVisibility);
        }
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            SetCursorState(cursorVisibility);
        }
    }

    private void SetCursorState(bool cursorVisibility)
    {
        Cursor.visible = cursorVisibility;
        Cursor.lockState = cursorVisibility ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
