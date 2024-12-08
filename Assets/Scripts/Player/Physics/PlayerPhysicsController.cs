using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Transform Body { get; private set; }

    [SerializeField] private EntityGravity entityGravity = new();

    public float GravityScale
    {
        get => entityGravity.gravityScale;
    }
    public bool IsGrounded
    {
        get => isGrounded;
    }

    public Vector3 Velocity { get; private set; }
    public Vector3 AccumulatedImpulse { get; private set; }

    private bool isGrounded;

    [SerializeField] private float impulseDamping = 0.9f;
    [SerializeField] private float impulseGroundDamping = 0.025f;

    [SerializeField] private List<IPhysicsComponent> physicsHandlers;


    public void ChangeForce(Vector3 force)
    {
        float upValue = force.y;
        force.y = 0;

        AccumulatedImpulse = force;
        entityGravity.gravityValue = upValue;
    }

    public void AddUpForce(float value)
    {
        entityGravity.gravityValue += value;
    }

    public void AddForce(Vector3 force)
    {
        float upValue = force.y;
        force.y = 0;

        AccumulatedImpulse += force;
        entityGravity.gravityValue += upValue;
    }

    void Start()
    {
        physicsHandlers = GetComponentsInChildren<IPhysicsComponent>().ToList();

        foreach (var handler in physicsHandlers)
        {
            handler.PlayerPhysicsController = this;
        }
        entityGravity.PlayerPhysicsController = this;
    }

    private void Update()
    {
        isGrounded = CharacterController.isGrounded;

        Vector3 velocity = Vector3.zero;
        foreach (IPhysicsComponent handler in physicsHandlers)
        {
            handler.CustomUpdate();
            velocity += handler.Velocity;
        }
        entityGravity.CustomUpdate();

        velocity += AccumulatedImpulse;
        velocity += entityGravity.Velocity;

        AccumulatedImpulseDamping();
        ApplyVelocity(velocity);
    }

    private void AccumulatedImpulseDamping()
    {
        float dampingFactor = Mathf.Pow(isGrounded ? impulseGroundDamping : impulseDamping, Time.deltaTime);
        AccumulatedImpulse *= dampingFactor;
    }

    private void ApplyVelocity(Vector3 velocity)
    {
        Velocity = velocity;
        CharacterController.Move(velocity);
    }
}
