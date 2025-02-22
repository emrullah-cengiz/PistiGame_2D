using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _settings;
    
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        
        //Settings
        builder.RegisterInstance(_settings.PlayerSettings).AsSelf();
        builder.RegisterInstance(_settings.LobbySettings).AsSelf();
        builder.RegisterInstance(_settings.CardSettings).AsSelf();
        builder.RegisterInstance(_settings.TableSessionSettings).AsSelf();
        
        //Other installers
        builder.RegisterComponentInHierarchy<TableSessionLifetimeScopeInstaller>().AsSelf();
        
        //Bootstrapper
        builder.RegisterEntryPoint<GameBootstrapper>();
        
        //Game States
        builder.Register<GameLoading_GameState>(Lifetime.Singleton).AsSelf();
        builder.Register<Lobby_GameState>(Lifetime.Singleton).AsSelf();
        builder.Register<TableSession_GameState>(Lifetime.Singleton).AsSelf();

        builder.Register<GameStateManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        
        //Systems
        builder.Register<PlayerDataSaveSystem>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<UIManager>().AsSelf().AsImplementedInterfaces();
        builder.Register<PlayerWallet>(Lifetime.Singleton).AsSelf();

    }
    
}