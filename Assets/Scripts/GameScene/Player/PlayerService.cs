using UnityEngine;

public class PlayerService : MonoBehaviour
{
    [SerializeField] private PlayerPhysicsController playerPhysicsController;
    [SerializeField] private PlayerBodyRotator bodyRotator;

    [field: SerializeField] public GameObject Player { get; private set; }

    public Vector3 Velocity
    {
        get => playerPhysicsController.Velocity;
    }

    public Quaternion BodyRotation
    {
        get => bodyRotator.Rotation;
    }
}
