using UnityEngine;
using UnityEngine.UI;

public class PlayerValuesVisualizer : MonoBehaviour
{
    [SerializeField] private PlayerMover playerMover;
    [SerializeField] private Toggle isGround;

    private void Update()
    {
        isGround.isOn = playerMover.GroundedPlayer;
    }
}
