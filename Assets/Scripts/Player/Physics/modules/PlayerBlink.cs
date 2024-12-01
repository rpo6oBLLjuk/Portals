using DG.Tweening;
using UnityEngine;
using Zenject;

public class PlayerBlink : MonoBehaviour, IPhysicsComponent
{
    [Inject] CameraService cameraService;

    public PlayerPhysicsController EntityPhysicsController { get; set; }
    public Vector3 Velocity { get; set; }

    public bool Reloaded => currentReloadTime <= 0;
    public bool Blinking { get; private set; }

    public float distance = 1f;
    public float blinkDuration = 0.25f;
    public float reloadTime = 1f;

    private float currentReloadTime = 0f;
    private Vector3 direction;

    public void CustomUpdate()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Reloaded)
        {
            StartBlink();
        }

        if (Blinking)
            Velocity = distance / blinkDuration * direction * Time.deltaTime;
    }

    private void StartBlink()
    {
        Blinking = true;
        direction = cameraService.Forward;

        DOVirtual.DelayedCall(blinkDuration, () =>
        {
            Velocity = Vector3.zero;
            StartReload();
        });
    }

    private void StartReload()
    {
        Blinking = false;
        currentReloadTime = reloadTime;

        DOTween.To(() => currentReloadTime, x => currentReloadTime = x, 0, reloadTime);
    }
}
