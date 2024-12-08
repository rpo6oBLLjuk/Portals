using UnityEngine;

public interface IPhysicsComponent
{
    public PlayerPhysicsController PlayerPhysicsController { get; set; }
    public Vector3 Velocity { get; }

    public void CustomUpdate();
}
