using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _settings;
    
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        
        //Utilities
        builder.Register<DataSaveHandler<PlayerData>>(Lifetime.Transient).AsSelf();
        
        //Settings
        builder.RegisterInstance(_settings).AsSelf();
        
        //Bootstrapper
        builder.RegisterEntryPoint<GameBootstrapper>();
        
        //Game States
        builder.Register<GameLoading_GameState>(Lifetime.Singleton).AsSelf();
        builder.Register<Lobby_GameState>(Lifetime.Singleton).AsSelf();
        builder.Register<Playing_GameState>(Lifetime.Singleton).AsSelf();
        builder.Register<TableSessionEnd_GameState>(Lifetime.Singleton).AsSelf();

        builder.Register<GameStateManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        
        //Systems
        builder.Register<PlayerDataSaveSystem>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<UIManager>().AsSelf().AsImplementedInterfaces();
        builder.Register<PlayerWallet>(Lifetime.Singleton).AsSelf();

    }

}
