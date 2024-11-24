using CustomInspector;
using System;
using UnityEngine;

[Serializable]
public class EntityGravity : IPhysicsComponent
{
    public EntityPhysicsController EntityPhysicsController { get; set; }
    public Vector3 Velocity
    {
        get => gravityValue * Time.deltaTime * Vector3.up;
    }

    [SerializeField] public float gravityScale = -9.81f;
    [SerializeField, ReadOnly] public float gravityValue;


    public void CustomUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (EntityPhysicsController.IsGrounded && gravityValue < 0)
        {
            gravityValue = gravityScale * Time.deltaTime;
            return;
        }

        gravityValue += gravityScale * Time.deltaTime;
    }
}
