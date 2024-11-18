using CustomInspector;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private PlayerBodyRotator bodyRotator;

    [SerializeField, SelfFill(true)] private CharacterController characterController;


    [SerializeField, Tab("Values")] private float playerSpeed = 2.0f;
    [SerializeField, Tab("Values")] private float jumpHeight = 1.0f;
    [SerializeField, Tab("Values")] private float gravityValue = -9.81f;

    [SerializeField, Tab("Debug"), ReadOnly] public Vector3 playerVelocity;
    [SerializeField, Tab("Debug"), ReadOnly] private float horizontalInput;
    [SerializeField, Tab("Debug"), ReadOnly] private float verticalInput;

    [field: SerializeField, Tab("Debug")] public bool GroundedPlayer { get; private set; }


    private void Update()
    {
        CheckGround();

        GetInput();

        SimpleMove();

        if (GroundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Jump();

        ApplyGravity();
    }

    private void CheckGround()
    {
        bool lastGround = GroundedPlayer;

        bool raycast = Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 - characterController.center.y + 0.01f);
        GroundedPlayer = characterController.isGrounded || raycast;

        if (lastGround != GroundedPlayer)
        {
            Debug.Log($"IsGround: {GroundedPlayer}, Raycast: {raycast}");
        }
    }

    private void SimpleMove()
    {
        Vector3 moveVector = bodyRotator.transform.forward * verticalInput + bodyRotator.transform.right * horizontalInput;
        moveVector.y = 0f;
        moveVector *= playerSpeed * Time.deltaTime;

        characterController.Move(moveVector);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && GroundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
}
