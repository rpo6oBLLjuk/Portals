using CustomInspector;
using UnityEngine;
using Zenject;

public class EffectServiceInstaller : MonoInstaller
{
    [SerializeField, SelfFill(true)] private EffectService effectService;

    public override void InstallBindings()
    {
        Container.Bind<EffectService>().FromInstance(effectService).AsSingle();
    }
}