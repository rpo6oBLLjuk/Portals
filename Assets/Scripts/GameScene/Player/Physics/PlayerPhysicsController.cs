using CustomInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [field: SerializeField, SelfFill(true)] public Rigidbody Rb { get; private set; }
    [field: SerializeField] public Transform Body { get; private set; }

    public float GravityScale
    {
        get => entityGravity.gravityScale;
    }
    public bool IsGrounded
    {
        get => isGrounded;
    }

    [field: SerializeField, HorizontalLine("Velocity")] public Vector3 Velocity { get; private set; }
    [field: SerializeField] public Vector3 AccumulatedImpulse { get; private set; }
    [field: SerializeField] public Vector3 MovementVelocity { get; private set; }

    [SerializeField, SelfFill, HorizontalLine("Ground check")] private CapsuleCollider capsuleCollider;
    [SerializeField, ShowIf(ComparisonOp.NotNull, (nameof(capsuleCollider)))] private Vector3 checksphereOffset;
    [SerializeField, ShowIf(ComparisonOp.NotNull, (nameof(capsuleCollider)))] private LayerMask checksphereLayerMask;
    [SerializeField, ReadOnly] private bool isGrounded;

    [SerializeField] private PlayerGravity entityGravity = new();

    [HorizontalLine("Damping")]
    [SerializeField] private float impulseDamping = 0.9f;
    [SerializeField] private float impulseGroundDamping = 0.025f;

    [SerializeField] private List<IPhysicsComponent> physicsHandlers;
    [SerializeField, ReadOnly] private float forceMultipluier = 100f;
    [SerializeField, ReadOnly] private List<Collider> ignoreColliders;


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

    public void IngoreCollision(List<Collider> colliders, bool ignore)
    {
        //string debug = "Ignore: ";
        //foreach (Collider collider in colliders)
        //{
        //    debug += $"{collider.name}, " ;
        //}
        //Debug.Log($"{debug} is {ignore}");

        if (ignore)
            ignoreColliders.AddRange(colliders);
        else
            ignoreColliders.Clear();
    }


    private void Start()
    {
        GetPhysicsComponents();
    }

    private void Update()
    {
        CheckGround();
        entityGravity.CustomUpdate();
        AccumulatedImpulseDamping();

        Vector3 velocity = Vector3.zero;
        foreach (IPhysicsComponent handler in physicsHandlers)
        {
            handler.CustomUpdate();
            velocity += handler.Velocity;
        }

        forceMultipluier = 1 / Time.deltaTime;
        velocity *= forceMultipluier * Time.deltaTime;
        MovementVelocity = velocity;

        AccumulatedImpulse = new Vector3(AccumulatedImpulse.x, entityGravity.Velocity.y, AccumulatedImpulse.z);   //!!!
        velocity += AccumulatedImpulse;

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
        Rb.velocity = velocity;
    }

    private void GetPhysicsComponents()
    {
        physicsHandlers = GetComponentsInChildren<IPhysicsComponent>().ToList();
        foreach (var handler in physicsHandlers)
        {
            handler.PlayerPhysicsController = this;
        }
        entityGravity.PlayerPhysicsController = this;
    }

    private void CheckGround()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + checksphereOffset, capsuleCollider.radius * 0.99999f, Vector3.down, 0.01f, checksphereLayerMask, QueryTriggerInteraction.Ignore);

        isGrounded = false;

        // Проверяем все пересечения
        foreach (RaycastHit hit in hits)
        {
            // Если коллайдер не в списке игнорируемых
            if (!ignoreColliders.Contains(hit.collider))
            {
                isGrounded = true;
                return; // Выходим из метода, находимся на земле
            }
        }

        // Если дошли до сюда, значит не на земле
        isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + checksphereOffset, capsuleCollider.radius * 0.99999f);
    }
}
