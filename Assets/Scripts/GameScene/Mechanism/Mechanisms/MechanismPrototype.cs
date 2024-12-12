using CustomInspector;
using System;
using UnityEngine;

public abstract class MechanismPrototype : MonoBehaviour, IMechanism
{
    public event Action Activate;
    public event Action Deactivate;

    [SerializeField] private bool logging = false;
    [SerializeField, ReadOnly] private bool isActive = false;

    protected void MechanismActivate()
    {
        if (isActive)
            return;

        Activate?.Invoke();
        isActive = true;

        if (logging)
            Debug.Log("Mechanism Activate", this);
    }
    protected void MechanismDeactivate()
    {
        if (!isActive)
            return;

        Deactivate?.Invoke();
        isActive = false;

        if (logging)
            Debug.Log("Mechanism Deactivate", this);
    }
}
