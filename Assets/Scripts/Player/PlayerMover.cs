using CustomInspector;
using UnityEngine;
using Zenject;

public class PlayerMover : MonoBehaviour
{
    [Inject] CameraService cameraService;
    [SerializeField, SelfFill(true)] private CharacterController characterController;

    [SerializeField] private float speed;

    [SerializeField, ReadOnly] private float horizontalInput;
    [SerializeField, ReadOnly] private float verticalInput;

    void Start()
    {

    }

    void Update()
    {
        GetInput();
        Move();

        if (Input.GetButtonDown("Jump"))
        {
            characterController.Move(Vector3.up);
        }
    }

    private void Move()
    {
        Vector3 moveVector = cameraService.MainCamera.transform.forward * verticalInput + cameraService.MainCamera.transform.right * horizontalInput;
        moveVector *= speed;

        characterController.SimpleMove(moveVector);
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
}
