using UnityEngine;

public class UIService : MonoBehaviour
{
    [field: SerializeField] public CrosshairController CrosshairController { get; private set; }
    [field: SerializeField] public ConsoleWidget ConsoleWidget { get; private set; }

}
