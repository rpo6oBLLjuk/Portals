using CustomInspector;
using UnityEngine;
using Zenject;

public class PortalServiceInstaller : MonoInstaller
{
    [SerializeField, SelfFill(true)] private PortalService portalService;

    public override void InstallBindings()
    {
        Container.Bind<PortalService>().FromInstance(portalService).AsSingle();
    }
}