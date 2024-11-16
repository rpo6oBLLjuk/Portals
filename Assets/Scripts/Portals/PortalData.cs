using CustomInspector;
using System;
using UnityEngine;

[Serializable]
public class PortalData
{
    [field: SerializeField, Hook(nameof(FindPortalCamera))] public Transform Portal { get; private set; }
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public Vector3 RotationOffset { get; private set; }


    private void FindPortalCamera()
    {
        Camera camera = Portal.GetComponentInChildren<Camera>();

        if (camera == null)
            Debug.Log("Не удалось найти камеру портала. Убедитесь, что она является дочерним объектом портала, или добавьте её вручную");
        else
            Camera = camera;
    }
}
