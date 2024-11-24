using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityPhysicsController : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [SerializeField] private EntityGravity entityGravity = new();

    public bool IsGrounded
    {
        get => isGrounded;
    }
    public float GravityScale
    {
        get => entityGravity.gravityScale;
    }
    public Vector3 Velocity { get; private set; }

    [SerializeField] private List<IPhysicsComponent> physicsHandlers;
    private bool isGrounded;

    public void AddUpForce(float value)
    {
        entityGravity.gravityValue += value;
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

        ApplyVelocity(velocity);
    }

    private void ApplyVelocity(Vector3 velocity)
    {
        Velocity = velocity;
        CharacterController.Move(velocity);
    }
}
