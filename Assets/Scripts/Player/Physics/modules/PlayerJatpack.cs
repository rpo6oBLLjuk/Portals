using CustomInspector;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class PlayerJatpack : MonoBehaviour, IPhysicsComponent
{
    public PlayerPhysicsController PlayerPhysicsController { get; set; }
    public Vector3 Velocity { get; set; }

    public static event Action<float> EnergyChanged;

    [SerializeField, Tab("Values")] private float duration = 1;
    [SerializeField, Tab("Values")] private bool scaleByGravity = true;
    [SerializeField, Tab("Values"), ShowIf(nameof(scaleByGravity))] private float forceMultiplier = 1.1f;
    [SerializeField, Tab("Values"), ShowIfNot(nameof(scaleByGravity))] private float force = 1;


    [SerializeField, Tab("Values")] private float reloadingDelay = 1;
    [SerializeField, Tab("Values")] private float reloadingDuration = 1;

    private float CurrentEnegry
    {
        get => currentEnergy;
        set
        {
            currentEnergy = Mathf.Clamp01(value);
            EnergyChanged?.Invoke(value);
        }
    }
    [SerializeField, Tab("Debug"), ReadOnly] private float currentEnergy = 1;
    [SerializeField, Tab("Debug"), ReadOnly] private bool isFly;
    [SerializeField, Tab("Debug"), ReadOnly] private float currentReloadingTime;


    public void CustomUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!PlayerPhysicsController.IsGrounded)
            {
                StartFly();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || PlayerPhysicsController.IsGrounded)
        {
            if (isFly)
            {
                EndFly();
            }
        }

        if (isFly)
        {
            Fly();
        }
        else
        {
            Reloading();
        }
    }

    private void StartFly()
    {
        if (CurrentEnegry == 0)
        {
            Debug.Log("Energy end");
            return;
        }

        isFly = true;

        currentReloadingTime = 0;

    }

    private void EndFly()
    {
        isFly = false;
    }

    private void Fly()
    {
        PlayerPhysicsController.AddUpForce(GetUpForce() * Time.deltaTime);

        SpendEnergy();
    }


    private void SpendEnergy()
    {
        CurrentEnegry -= Time.deltaTime / duration;

        if (CurrentEnegry == 0)
        {
            EndFly();
        }
    }

    private void Reloading()
    {
        if (currentReloadingTime < 1)
            currentReloadingTime = Mathf.Clamp01(currentReloadingTime + Time.deltaTime / reloadingDelay);
        else
            CurrentEnegry += Time.deltaTime / reloadingDuration;

    }

    private float GetUpForce()
    {
        return (scaleByGravity) ? Mathf.Abs(PlayerPhysicsController.GravityScale) * forceMultiplier : force;
    }
}
