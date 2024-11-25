using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PortalGunStateController : MonoBehaviour
{
    [Inject] DiContainer container;
    [Inject] PortalService portalService;
    [Inject] UIService uiService;

    [SerializeField] private PortalsState portalsState = new();
    [SerializeField] private PickUpState pickupState = new();

    [SerializeField] private PortalGunState currentState;
    [SerializeField] private List<PortalGunState> states = new();


    private void Start()
    {
        container.Inject(portalsState);
        container.Inject(pickupState);

        states.Add(portalsState);
        states.Add(pickupState);

        currentState = portalsState;
    }

    private void Update()
    {
        PortalGunState newState = GetActiveState();
        if (currentState != newState)
        {
            SwitchState(newState);
        }

        currentState?.Update();
    }

    private void SwitchState(PortalGunState newState)
    {
        currentState?.DeactivateState();
        newState?.ActivateState();
        currentState = newState;
    }

    private PortalGunState GetActiveState()
    {
        if (pickupState.PickedUp)
            return pickupState;

        foreach(var state in states)
        {
            if (state.CheckRaycast())
                return state;
        }

        return null;
    }
}
