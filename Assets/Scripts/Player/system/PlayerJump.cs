using UnityEngine;

public class PlayerJump : MonoBehaviour, IPhysicsComponent
{
    public EntityPhysicsController EntityPhysicsController { get; set; }
    public Vector3 Velocity
    {
        get => Vector3.zero;
    }

    [SerializeField] private float jumpHeight = 1.0f;


    public void CustomUpdate()
    {
        Jump();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && EntityPhysicsController.IsGrounded)
        {
            EntityPhysicsController.AddUpForce(Mathf.Sqrt(jumpHeight * -1.0f * EntityPhysicsController.GravityScale));
            return;
        }
    }
}
