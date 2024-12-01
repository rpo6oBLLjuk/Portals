using UnityEngine;

public abstract class MechanismResponderPrototype : MonoBehaviour
{
    [field: SerializeField] public MechanismPrototype mechanismPrototype { get; protected set; }

    
    public abstract void Activated();
    public abstract void Deactivated();


    private void OnEnable()
    {
        mechanismPrototype.Activate += Activated;
        mechanismPrototype.Deactivate += Deactivated;
    }

    private void OnDisable()
    {
        mechanismPrototype.Activate -= Activated;
        mechanismPrototype.Deactivate -= Deactivated;
    }
}
