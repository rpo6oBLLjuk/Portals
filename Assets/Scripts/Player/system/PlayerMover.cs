using CustomInspector;
using UnityEngine;

public class PlayerMover : MonoBehaviour, IPhysicsComponent
{
    [SerializeField] private PlayerBodyRotator bodyRotator; //bad practice

    public EntityPhysicsController EntityPhysicsController { get; set; }
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
        Vector3 moveVector = bodyRotator.transform.forward * verticalInput + bodyRotator.transform.right * horizontalInput;
        moveVector.y = 0f;
        moveVector *= playerSpeed * Time.deltaTime;

        Velocity = moveVector;
    }
}
