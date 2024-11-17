using CustomInspector;
using System;
using UnityEngine;

[Serializable]
public class PortalData
{
    [field: SerializeField, Hook(nameof(FindPortalComponents))] public Transform PortalContainer { get; private set; }
    [field: SerializeField] public Transform PortalMesh { get; private set; }
    [field: SerializeField] public Camera Camera { get; private set; }

    [field: SerializeField] private float spawnOffset = 0.0001f;


    public void PlacePortal(RaycastHit hit)
    {
        // ������� �����������
        Vector3 normal = hit.normal;

        // ���������� ����� �������
        Vector3 up;

        // ���� ����������� �������������� (���/�������), ��������� �����
        if (Mathf.Abs(Vector3.Dot(normal, Vector3.up)) > 0.99f)
        {
            up = Vector3.forward; // ��� �������������� ������������ ����� ������������� ����
        }
        else
        {
            // ��� ��������� ������������ "����" ��������� ����� �����-�������
            up = Vector3.Cross(Vector3.right, normal).normalized;

            // ���� up ������� ���, ���������� �������� ������
            if (up.sqrMagnitude < 0.001f)
            {
                up = Vector3.Cross(Vector3.forward, normal).normalized;
            }

            // ��������� ����������� up ������������ ����������� Vector3.up
            if (Vector3.Dot(up, Vector3.up) < 0)
            {
                up = -up; // ��������������, ���� "����" ��������� ����
            }
        }

        // ������������� �������� �������
        Quaternion portalRotation = Quaternion.LookRotation(-normal, up);

        // ����� ������� � ������� �������
        PortalContainer.position = hit.point + normal * spawnOffset;
        PortalContainer.rotation = portalRotation;
    }


    private void FindPortalComponents()
    {
        Camera camera = PortalContainer.GetComponentInChildren<Camera>();
        if (camera == null)
            Debug.LogWarning("�� ������� ����� ������ �������. ���������, ��� ��� �������� �������� �������� ���������� �������, ��� �������� � �������");
        else
            Camera = camera;

        MeshRenderer mesh = PortalContainer.GetComponentInChildren<MeshRenderer>();
        if (mesh == null)
            Debug.LogWarning("�� ������� ����� ��� �������. ���������, ��� �� �������� �������� �������� ���������� �������, ��� �������� ��� �������");
        else
            PortalMesh = mesh.transform;
    }
}
