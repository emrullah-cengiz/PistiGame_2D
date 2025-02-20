using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameBootstrapper : IPostStartable
{
    [Inject] private GameStateManager _gameStateManager;

    public void PostStart()
    {
        _gameStateManager.Initialize();

        // InitializeGameStateManagerDelayed().Forget();
    }

    async UniTask InitializeGameStateManagerDelayed()
    {
        await UniTask.Yield();
        _gameStateManager.Initialize();
    }
}