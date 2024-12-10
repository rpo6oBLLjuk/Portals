using UnityEngine;

public class CameraService : MonoBehaviour
{
    [field: SerializeField] public Camera MainCamera { get; private set; }
    public Vector3 Forward { get; private set; }
    public Vector3 Right { get; private set; }

    [SerializeField] private bool cursorVisibility = false;


    public void SetCameraLocalPosition(Vector3 localPosition)
    {
        MainCamera.transform.localPosition = localPosition;
    }


    private void Start()
    {
        SetCursorState(cursorVisibility);

        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SetCursorState(!cursorVisibility);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            SetCursorState(cursorVisibility);
        }

        CalculatePublicFields();
    }

    private void CalculatePublicFields()
    {
        Quaternion rotation = MainCamera.transform.rotation; // Ваш кватернион вращения
        rotation.y = 0;

        // Получаем forward и right векторы
        Vector3 forward = rotation * Vector3.forward;
        Vector3 right = rotation * Vector3.right;

        // Нормализуем векторы
        Forward = forward.normalized;
        Right = right.normalized;
    }

    private void SetCursorState(bool cursorVisibility)
    {
        Cursor.visible = cursorVisibility;
        Cursor.lockState = cursorVisibility ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
