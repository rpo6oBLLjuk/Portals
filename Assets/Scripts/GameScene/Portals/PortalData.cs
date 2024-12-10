using CustomInspector;
using System;
using UnityEngine;

[Serializable]
public class PortalData
{
    [field: SerializeField, Hook(nameof(FindPortalComponents))] public Transform Container { get; private set; }
    [field: SerializeField] public Transform RenderMesh { get; private set; }
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public PortalWarpController WarpController { get; private set; }

    [field: SerializeField, ReadOnly] public RenderTexture RenderTexture {  get; private set; }

    [field: SerializeField] private float spawnOffset = 0.0001f;


    public void Start(RenderTexture renderTexture, Camera mainCamera)
    {
        this.RenderTexture =  renderTexture;

        Camera.CopyFrom(Camera);
        RenderMesh.GetComponent<MeshRenderer>().material.mainTexture = renderTexture;
    }

    public void PlacePortal(RaycastHit hit)
    {
        // Нормаль поверхности
        Vector3 normal = hit.normal;

        // Определяем вверх портала
        Vector3 up;

        // Если поверхность горизонтальная (пол/потолок), фиксируем вверх
        if (Mathf.Abs(Vector3.Dot(normal, Vector3.up)) > 0.99f)
        {
            up = Vector3.forward; // Для горизонтальных поверхностей задаём фиксированный верх
        }
        else
        {
            // Для наклонных поверхностей "верх" вычисляем через кросс-продукт
            up = Vector3.Cross(Vector3.right, normal).normalized;

            // Если up слишком мал, используем запасной вектор
            if (up.sqrMagnitude < 0.001f)
            {
                up = Vector3.Cross(Vector3.forward, normal).normalized;
            }

            // Проверяем направление up относительно глобального Vector3.up
            if (Vector3.Dot(up, Vector3.up) < 0)
            {
                up = -up; // Переворачиваем, если "верх" направлен вниз
            }
        }

        // Устанавливаем вращение портала
        Quaternion portalRotation = Quaternion.LookRotation(-normal, up);

        // Задаём позицию и поворот портала
        Container.position = hit.point + normal * spawnOffset;
        Container.rotation = portalRotation;
    }


    private void FindPortalComponents()
    {
        Camera camera = Container.GetComponentInChildren<Camera>();
        if (camera == null)
            Debug.LogWarning("Не удалось найти камеру портала. Убедитесь, что она является дочерним объектом контейнера портала, или добавьте её вручную");
        else
            Camera = camera;

        MeshRenderer mesh = Container.GetComponentInChildren<MeshRenderer>();
        if (mesh == null)
            Debug.LogWarning("Не удалось найти меш портала. Убедитесь, что он является дочерним объектом контейнера портала, или добавьте его вручную");
        else
            RenderMesh = mesh.transform;
    }
}
