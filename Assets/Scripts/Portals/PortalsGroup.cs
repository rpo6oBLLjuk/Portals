using CustomInspector;
using System;
using UnityEngine;

[Serializable]
public class PortalsGroup
{
    [field: SerializeField] public PortalData FirstPortalData { get; private set; }
    [field: SerializeField] public PortalData SecondPortalData { get; private set; }

    public Quaternion RotationBetweenPortals { get; private set; }

    public void Start()
    {
        if (FirstPortalData == null)
            Debug.LogError("First Portal is empty");

        if (SecondPortalData == null)
            Debug.LogError("Second Portal is empty");
    }

    public void Update(Transform playerCamera)
    {
        UpdatePortalCamera(FirstPortalData.PortalContainer, SecondPortalData.PortalContainer, FirstPortalData.Camera, playerCamera);
        UpdatePortalCamera(SecondPortalData.PortalContainer, FirstPortalData.PortalContainer, SecondPortalData.Camera, playerCamera);

        CalculateRotationBetweenPortals();
    }

    private void UpdatePortalCamera(Transform fromPortal, Transform toPortal, Camera portalCamera, Transform playerCamera)
    {
        // ������������ ������� ������ "��" �������
        Vector3 relativePosition = fromPortal.InverseTransformPoint(playerCamera.position);
        relativePosition = -relativePosition;
        relativePosition.y = -relativePosition.y;

        Vector3 newCameraPosition = toPortal.TransformPoint(relativePosition);
        portalCamera.transform.position = newCameraPosition;

        // ������������ ����������� ������, "������ �����" �� �������
        Vector3 relativeDirection = fromPortal.InverseTransformDirection(playerCamera.forward);
        relativeDirection.y = -relativeDirection.y;
        Vector3 newCameraDirection = toPortal.TransformDirection(relativeDirection);

        // ������������ "�����" ������
        Vector3 relativeUp = fromPortal.InverseTransformDirection(playerCamera.up);
        Vector3 newCameraUp = toPortal.TransformDirection(relativeUp);

        // ������������� ���������� ������, �������� ������� �� 180 ��������
        Quaternion portalRotation = Quaternion.LookRotation(newCameraDirection, newCameraUp);
        portalCamera.transform.rotation = portalRotation * Quaternion.Euler(0, 180, 0);

        // ������������� nearClippingPlane
        float portalThickness = (newCameraPosition - toPortal.position).magnitude;
        portalCamera.nearClipPlane = portalThickness;
    }

    private void CalculateRotationBetweenPortals()
    {
        RotationBetweenPortals = Quaternion.Inverse(FirstPortalData.PortalContainer.rotation) * SecondPortalData.PortalContainer.rotation;
    }

    //private void UpdatePortalCamera(PortalData fromPortal, PortalData toPortal, Transform playerCamera)
    //{
    //    //// ��������� ������� ������ � ������ �������� �������
    //    //Vector3 localOffset = fromPortal.PortalContainer.InverseTransformPoint(playerCamera.position);
    //    //Vector3 targetPosition = toPortal.PortalContainer.TransformPoint(-localOffset);

    //    //toPortal.Camera.transform.position = targetPosition;

    //    //Vector3 offset = playerCamera.position - fromPortal.PortalContainer.position;
    //    //toPortal.Camera.transform.position = toPortal.PortalContainer.position + offset;
    //    //toPortal.Camera.transform.position = new Vector3(toPortal.Camera.transform.position.x, playerCamera.transform.position.y, toPortal.Camera.transform.position.z);

    //    Vector3 relativePosition = fromPortal.PortalContainer.InverseTransformPoint(Camera.main.transform.position);
    //    Vector3 newCameraPosition = toPortal.PortalContainer.TransformPoint(relativePosition);
    //    fromPortal.Camera.transform.position = newCameraPosition;

    //    //// ����������� ������� �������
    //    //Quaternion portalRotation = Quaternion.Inverse(toPortal.PortalContainer.rotation);

    //    //// ������ ������ �������� � ��������������� ������� �� �������, ����������� ����������� �������
    //    //Quaternion cameraRotation = Quaternion.LookRotation(-toPortal.PortalContainer.forward, toPortal.PortalContainer.up);

    //    //// ��������� �������������� ��������
    //    //Quaternion additionalRotation = Quaternion.AngleAxis(additionalRotationDegrees, toPortal.PortalContainer.forward);

    //    // ������������� ������������� �������� ������
    //    toPortal.Camera.transform.localRotation = Quaternion.Euler(localRotation);

    //    // ��������� nearClipPlane
    //    float clipDistance = Vector3.Distance(toPortal.PortalContainer.position, playerCamera.position);
    //    toPortal.Camera.nearClipPlane = clipDistance;
    //}

}
