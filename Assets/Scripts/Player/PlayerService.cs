using UnityEngine;

public class PlayerService : MonoBehaviour
{
    [SerializeField] private PlayerMover playerMover;
    [SerializeField] private PlayerBodyRotator bodyRotator;

    public Vector3 Velocity
    {
        get => playerMover.playerVelocity;
    }

    public Quaternion BodyRotation
    {
        get => bodyRotator.rotation;
    }

}
