using CustomInspector;
using UnityEngine;
using Zenject;

public class PlayerMover : MonoBehaviour
{
    [Inject] CameraService cameraService;
    [SerializeField, SelfFill(true)] private CharacterController characterController;


    [SerializeField, Tab("Values")] private float playerSpeed = 2.0f;
    [SerializeField, Tab("Values")] private float jumpHeight = 1.0f;
    [SerializeField, Tab("Values")] private float gravityValue = -9.81f;

    [SerializeField, Tab("Debug"), ReadOnly] private Vector3 playerVelocity;
    [SerializeField, Tab("Debug"), ReadOnly] private float horizontalInput;
    [SerializeField, Tab("Debug"), ReadOnly] private float verticalInput;

    public bool GroundedPlayer;
    //[SerializeField, Tab("Debug"), ReadOnly] private bool groundedPlayer;


    private void Update()
    {
        bool lastGround = GroundedPlayer;

        bool raycast = Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 - characterController.center.y + 0.01f);
        GroundedPlayer = characterController.isGrounded || raycast;

        if (lastGround != GroundedPlayer)
        {
            Debug.Log($"IsGround: {GroundedPlayer}, Raycast: {raycast}");
        }

        GetInput();

        SimpleMove();

        if (GroundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Jump();

        ApplyGravity();
    }

    private void SimpleMove()
    {
        Vector3 moveVector = cameraService.MainCamera.transform.forward * verticalInput + cameraService.MainCamera.transform.right * horizontalInput;
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
