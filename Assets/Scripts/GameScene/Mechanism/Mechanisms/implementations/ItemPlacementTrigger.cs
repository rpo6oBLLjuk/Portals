using System.Collections.Generic;
using UnityEngine;

public class ItemPlacementTrigger : MechanismPrototype
{
    [SerializeField] private LayerMask triggerLayers;

    [SerializeField] private List<Collider> colliders = new();


    private void OnTriggerEnter(Collider other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer)))
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
