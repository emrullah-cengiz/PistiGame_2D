using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        
        builder.Register<GameStateManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        
    }

}
