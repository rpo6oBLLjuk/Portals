using System;
using UnityEngine;

public abstract class MechanismPrototype : MonoBehaviour, IMechanism
{
    public event Action Activate;
    public event Action Deactivate;

    protected void MechanismActivate()
    {
        Activate?.Invoke();

        Debug.Log("Mechanism Activate", this);
    }
    protected void MechanismDeactivate()
    {
        Deactivate?.Invoke();

        Debug.Log("Mechanism Deactivate", this);
    }
}
