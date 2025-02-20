using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameBootstrapper : MonoBehaviour
{
    [Inject] private GameStateManager _gameStateManager;

    private void Start()
    {
        _gameStateManager.Initialize();
    }
}
