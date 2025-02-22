using System.Collections.Generic;
using GAME.Utilities.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public enum GameState
{
    GameLoading,
    Lobby,
    TableSession,
}

public class GameStateManager : ITickable
{
    private StateMachine<GameState> _stateMachine;

    [Inject] private GameLoading_GameState _gameLoading_GameState;
    [Inject] private Lobby_GameState _lobby_GameState;
    [Inject] private TableSession_GameState _tableSessionGameState;

    public void Initialize()
    {
        Debug.Log("GameStateManager::Initialize");
        
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new StateMachine<GameState>();

        _stateMachine.OnStateChanged += OnStateChanged;

        _stateMachine.AddState(GameState.GameLoading, _gameLoading_GameState);
        _stateMachine.AddState(GameState.Lobby, _lobby_GameState);
        _stateMachine.AddState(GameState.TableSession, _tableSessionGameState);

        _stateMachine.SetStartState(GameState.GameLoading);
        _stateMachine.Init();
    }

    private void OnStateChanged(GameState state)
    {
        Debug.Log($"Game state changing.. {_stateMachine.CurrentState} > {state}");
        Event.OnGameStateChanged?.Invoke(state);
    }

    public void Tick()
    {
        _stateMachine.Update();
    }
}


public abstract class GameStateBase : StateBase<GameState>
{
}