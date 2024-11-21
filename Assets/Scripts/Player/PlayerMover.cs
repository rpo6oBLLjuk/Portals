using CustomInspector;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private PlayerBodyRotator bodyRotator;
    [SerializeField, SelfFill(true)] public CharacterController characterController;

    [SerializeField, Tab("Values")] private float playerSpeed = 2.0f;
    [SerializeField, Tab("Values")] private float jumpHeight = 1.0f;
    [SerializeField, Tab("Values")] private float gravityValue = -9.81f;

    [SerializeField, Tab("Debug"), ReadOnly] private float horizontalInput;
    [SerializeField, Tab("Debug"), ReadOnly] private float verticalInput;

    [field: SerializeField, Tab("Debug"), ReadOnly] public float playerGravity { get; private set; }
    [field: SerializeField, Tab("Debug"), ReadOnly] public bool GroundedPlayer { get; private set; }
    [SerializeField, Tab("Debug")] private bool groundedLogs = false;

    [SerializeField, Tab("Debug")] private bool updateBreaked = false;


    public void Update()
    {
        CheckGround();

        GetInput();

        SimpleMove();

        if (GroundedPlayer && playerGravity < 0)
        {
            playerGravity = 0f;
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
            if (groundedLogs)
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
            playerGravity += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private void ApplyGravity()
    {
        playerGravity += gravityValue * Time.deltaTime;
        characterController.Move(playerGravity * Time.deltaTime * Vector3.up);
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    public void BreakUpdate()
    {
        updateBreaked = true;
        Debug.Log("Update breaked");
    }
}
