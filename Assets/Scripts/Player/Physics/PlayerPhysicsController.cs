using CustomInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [field: SerializeField, SelfFill(true)] public Rigidbody rb { get; private set; }
    [SerializeField, SelfFill] private CapsuleCollider capsuleCollider;
    [SerializeField, ShowIf(ComparisonOp.NotNull, (nameof(capsuleCollider)))] private Vector3 checksphereOffset;
    [SerializeField, ShowIf(ComparisonOp.NotNull, (nameof(capsuleCollider)))] private LayerMask checksphereLayerMask;

    [field: SerializeField] public Transform Body { get; private set; }

    [SerializeField] private PlayerGravity entityGravity = new();

    public float GravityScale
    {
        get => entityGravity.gravityScale;
    }
    public bool IsGrounded
    {
        get => isGrounded;
    }

    [field: SerializeField] public Vector3 Velocity { get; private set; }
    [field: SerializeField] public Vector3 AccumulatedImpulse { get; private set; }

    [SerializeField, ReadOnly] private bool isGrounded;

    [SerializeField] private float forceMultipluier = 100f;

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
        entityGravity.CustomUpdate();

        AccumulatedImpulseDamping();

        isGrounded = Physics.CheckSphere(transform.position + checksphereOffset, capsuleCollider.radius * 0.99999f, checksphereLayerMask, QueryTriggerInteraction.Ignore);


        Vector3 velocity = Vector3.zero;
        foreach (IPhysicsComponent handler in physicsHandlers)
        {
            handler.CustomUpdate();
            velocity += handler.Velocity;
        }

        velocity *= forceMultipluier * Time.deltaTime;
        velocity += AccumulatedImpulse;
        velocity += entityGravity.Velocity;

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
        rb.velocity = velocity;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + checksphereOffset, capsuleCollider.radius * 0.99999f);
    }
}
