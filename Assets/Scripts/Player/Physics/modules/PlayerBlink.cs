using DG.Tweening;
using UnityEngine;

public class PlayerBlink : MonoBehaviour, IPhysicsComponent
{
    public PlayerPhysicsController PlayerPhysicsController { get; set; }
    public Vector3 Velocity { get; set; }

    public bool Reloaded => currentReloadTime <= 0;
    public bool Blinking { get; private set; }

    public float distance = 1f;
    public float blinkDuration = 0.25f;
    public float reloadTime = 1f;

    private float currentReloadTime = 0f;

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
        {
            Vector3 direction = PlayerPhysicsController.Velocity;
            direction.y = 0f;
            direction = direction.normalized;

            Velocity = distance / blinkDuration * direction * Time.deltaTime;
        }
    }

    private void StartBlink()
    {
        Blinking = true;

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
