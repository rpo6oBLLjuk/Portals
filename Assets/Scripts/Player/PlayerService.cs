using UnityEngine;

public class PlayerService : MonoBehaviour
{
    [SerializeField] private PlayerMover playerMover;
    [SerializeField] private PlayerBodyRotator bodyRotator;

    [field: SerializeField] public GameObject Player { get; private set; }

    public float Velocity
    {
        get => playerMover.playerGravity;
    }

    public Quaternion BodyRotation
    {
        get => bodyRotator.rotation;
    }
}
