using UnityEngine;

public class PlayerJump : MonoBehaviour, IPhysicsComponent
{
    public PlayerPhysicsController PlayerPhysicsController { get; set; }
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
        if (Input.GetButtonDown("Jump") && PlayerPhysicsController.IsGrounded)
        {
            PlayerPhysicsController.AddUpForce(Mathf.Sqrt(jumpHeight * -3.0f * PlayerPhysicsController.GravityScale));
            return;
        }
    }
}
