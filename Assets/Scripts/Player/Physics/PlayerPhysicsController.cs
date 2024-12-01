using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [SerializeField] private EntityGravity entityGravity = new();

    public bool IsGrounded
    {
        get => isGrounded;
    }
    private bool isGrounded;

    public float GravityScale
    {
        get => entityGravity.gravityScale;
    }

    public Vector3 Velocity { get; private set; }

    private Vector3 accumulatedImpulse = Vector3.zero;
    [SerializeField] private float impulseDamping = 0.9f;
    [SerializeField] private float impulseGroundDamping = 0.25f;

    [SerializeField] private List<IPhysicsComponent> physicsHandlers;


    public void AddUpForce(float value)
    {
        entityGravity.gravityValue += value;
    }

    public void AddForce(Vector3 force)
    {
        entityGravity.gravityValue += force.y;
        force.y = 0;
        accumulatedImpulse += force;
    }

    void Start()
    {
        physicsHandlers = GetComponentsInChildren<IPhysicsComponent>().ToList();
        physicsHandlers.Add(entityGravity);

        foreach (var handler in physicsHandlers)
        {
            handler.EntityPhysicsController = this;
        }
        //entityGravity.EntityPhysicsController = this;
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

        velocity += accumulatedImpulse;

        float dampingFactor = Mathf.Pow(isGrounded ? impulseGroundDamping : impulseDamping, Time.deltaTime);
        accumulatedImpulse *= dampingFactor;

        ApplyVelocity(velocity);
    }

    private void ApplyVelocity(Vector3 velocity)
    {
        Velocity = velocity;
        CharacterController.Move(velocity);
    }
}
