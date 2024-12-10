using CustomInspector;
using UnityEngine;
using Zenject;

public class UIServiceInstaller : MonoInstaller
{
    [SerializeField, SelfFill(true)] private UIService uiService;

    public override void InstallBindings()
    {
        Container.Bind<UIService>().FromInstance(uiService).AsSingle();
    }
}