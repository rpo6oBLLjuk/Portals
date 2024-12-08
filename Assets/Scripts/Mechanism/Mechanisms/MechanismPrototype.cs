using CustomInspector;
using System;
using UnityEngine;

public abstract class MechanismPrototype : MonoBehaviour, IMechanism
{
    public event Action Activate;
    public event Action Deactivate;

    [SerializeField, ReadOnly] private bool isActive = false;

    protected void MechanismActivate()
    {
        if (isActive)
            return;

        Activate?.Invoke();
        isActive = true;

        Debug.Log("Mechanism Activate", this);
    }
    protected void MechanismDeactivate()
    {
        if (!isActive)
            return;

        Deactivate?.Invoke();
        isActive = false;

        Debug.Log("Mechanism Deactivate", this);
    }
}
