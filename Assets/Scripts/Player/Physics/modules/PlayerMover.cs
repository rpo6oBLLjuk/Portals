using CustomInspector;
using UnityEngine;
using Zenject;

public class PlayerMover : MonoBehaviour, IPhysicsComponent
{
    [Inject] CameraService cameraService;

    public PlayerPhysicsController EntityPhysicsController { get; set; }
    public Vector3 Velocity { get; set; }

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField, ReadOnly] private float horizontalInput;
    [SerializeField, ReadOnly] private float verticalInput;


    public void CustomUpdate()
    {
        GetInput();
        SimpleMove();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void SimpleMove()
    {
        Vector3 moveVector = cameraService.Forward * verticalInput + cameraService.Right * horizontalInput;
        moveVector *= playerSpeed * Time.deltaTime;

        Velocity = moveVector;
    }
}
