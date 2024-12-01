using CustomInspector;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacementTrigger : MechanismPrototype
{
    [SerializeField, Layer] private int triggerLayer;

    [SerializeField] private List<Collider> colliders = new();


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == triggerLayer)
        {
            if (colliders.Count == 0)
                MechanismActivate();

            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);

        if (colliders.Count == 0)
            MechanismDeactivate();
    }
}
