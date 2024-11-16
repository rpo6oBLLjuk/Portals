using System;
using UnityEngine;

[Serializable]
public class PortalsGroup
{
    [field: SerializeField] public PortalData FirstPortalData { get; private set; }
    [field: SerializeField] public PortalData SecondPortalData { get; private set; }


    public void Start()
    {
        if (FirstPortalData == null)
            Debug.LogError("First Portal is empty");

        if (SecondPortalData == null)
            Debug.LogError("Second Portal is empty");
    }

    public void Update(Transform playerCamera)
    {
        UpdatePortal(FirstPortalData, SecondPortalData, playerCamera);
        UpdatePortal(SecondPortalData, FirstPortalData, playerCamera);
    }

    private void UpdatePortal(PortalData fromPortal, PortalData toPortal, Transform playerCamera)
    {
        // Обновление позиции камеры
        Vector3 offset = playerCamera.position - fromPortal.Portal.position;
        toPortal.Camera.transform.position = toPortal.Portal.position - offset;

        // Обновление вращения камеры
        //Quaternion rotationDifference = toPortal.Portal.rotation * Quaternion.Inverse(fromPortal.Portal.rotation);
        //Quaternion rotationOffset = Quaternion.Euler(toPortal.RotationOffset); // Применяем корректировку
        //toPortal.Camera.transform.rotation = rotationDifference * playerCamera.rotation * rotationOffset;
    }
}
