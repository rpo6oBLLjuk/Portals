using CustomInspector;
using System;
using UnityEngine;

[Serializable]
public class PortalsGroup
{
    [field: SerializeField] public PortalData FirstPortalData { get; private set; }
    [field: SerializeField] public PortalData SecondPortalData { get; private set; }

    [field: SerializeField, FixedValues(0, 16, 24, 32)] private int RenderBufferDepth;
    [field: SerializeField, Range(0.1f, 2)] private float renderQualityMultiplier = 1f;

    public void Start(CameraService cameraService)
    {
        if (FirstPortalData == null)
        {
            Debug.LogError("First Portal is empty");
            return;
        }

        if (SecondPortalData == null)
        {
            Debug.LogError("Second Portal is empty");
            return;
        }

        int width = (int)(cameraService.MainCamera.pixelWidth * renderQualityMultiplier);
        int height = (int)(cameraService.MainCamera.pixelHeight * renderQualityMultiplier);

        Debug.Log($"Portal camera Width: {width}, Height: {height}");

        RenderTexture renderTexture1 = new(width, height, RenderBufferDepth);
        RenderTexture renderTexture2 = new(width, height, RenderBufferDepth);

        FirstPortalData.Start(renderTexture1, cameraService.MainCamera);
        SecondPortalData.Start(renderTexture2, cameraService.MainCamera);

        FirstPortalData.Camera.targetTexture = renderTexture2;
        SecondPortalData.Camera.targetTexture = renderTexture1;
    }

    public void Update(Transform playerCamera)
    {
        UpdatePortalCamera(FirstPortalData.Container, SecondPortalData.Container, SecondPortalData.Camera, playerCamera);
        UpdatePortalCamera(SecondPortalData.Container, FirstPortalData.Container, FirstPortalData.Camera, playerCamera);
    }

    private void UpdatePortalCamera(Transform fromPortal, Transform toPortal, Camera portalCamera, Transform playerCamera)
    {
        Vector3 loockerPosition = fromPortal.worldToLocalMatrix.MultiplyPoint3x4(playerCamera.position);
        loockerPosition.x *= -1;
        loockerPosition.z *= -1;

        Quaternion diffrence = toPortal.rotation * Quaternion.Inverse(fromPortal.rotation * Quaternion.Euler(0, 180, 0));

        portalCamera.transform.rotation = diffrence * playerCamera.rotation;
        portalCamera.transform.localPosition = loockerPosition;

        Plane p = new Plane(-toPortal.forward, toPortal.position);
        Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlane;

        var newMatrix = playerCamera.GetComponent<Camera>().CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = newMatrix;

        float portalThickness = (portalCamera.transform.localPosition).magnitude;
        portalCamera.nearClipPlane = portalThickness * 2;
    }
}
