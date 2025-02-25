using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameBootstrapper : IPostStartable, ITickable
{
    [Inject] private GameStateManager _gameStateManager;
    public void PostStart() => _gameStateManager.Initialize();
    
    
    [Inject] private IObjectResolver _objectResolver;
    public void Tick()
    {
        
    }
}