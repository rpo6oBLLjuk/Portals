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
        Container.position = hit.point + normal * spawnOffset;
        Container.rotation = portalRotation;
    }


    private void FindPortalComponents()
    {
        Camera camera = Container.GetComponentInChildren<Camera>();
        if (camera == null)
            Debug.LogWarning("�� ������� ����� ������ �������. ���������, ��� ��� �������� �������� �������� ���������� �������, ��� �������� � �������");
        else
            Camera = camera;

        MeshRenderer mesh = Container.GetComponentInChildren<MeshRenderer>();
        if (mesh == null)
            Debug.LogWarning("�� ������� ����� ��� �������. ���������, ��� �� �������� �������� �������� ���������� �������, ��� �������� ��� �������");
        else
            RenderMesh = mesh.transform;
    }
}
