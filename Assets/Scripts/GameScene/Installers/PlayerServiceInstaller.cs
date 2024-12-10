using CustomInspector;
using UnityEngine;
using Zenject;

public class PlayerServiceInstaller : MonoInstaller
{
    [SerializeField, SelfFill(true)] private PlayerService playerService;

    public override void InstallBindings()
    {
        Container.Bind<PlayerService>().FromInstance(playerService).AsSingle();
    }
}