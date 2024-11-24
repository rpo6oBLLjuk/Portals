using UnityEngine;

public interface IPhysicsComponent
{
    public EntityPhysicsController EntityPhysicsController { get; set; }
    public Vector3 Velocity { get; }

    public void CustomUpdate();
}
