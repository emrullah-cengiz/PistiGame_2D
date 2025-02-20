using GAME.Utilities.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public enum GameState
{
    GameLoading,
    Lobby,
    Playing,
    TableSessionEnd
}

public class GameStateManager : IInitializable, ITickable
{
    private StateMachine<GameState> _stateMachine;

    public void Initialize()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new StateMachine<GameState>();

        _stateMachine.AddState(GameState.GameLoading, new GameLoading_GameState());
        _stateMachine.AddState(GameState.Lobby, new Lobby_GameState());
        _stateMachine.AddState(GameState.Playing, new Playing_GameState());
        _stateMachine.AddState(GameState.TableSessionEnd, new TableSessionEnd_GameState());

        _stateMachine.SetStartState(GameState.GameLoading);
        _stateMachine.Init();
    }

    public void ChangeState(GameState state, params object[] @params)
    {
        _stateMachine.ChangeState(state, @params);

        Event.OnGameStateChanged?.Invoke(state);
    }

    public void Tick()
    {
        _stateMachine.Update();
    }
}


public abstract class GameStateBase : StateBase<GameState>
{
    [Inject] protected readonly GameStateManager StateManager;
}

public class GameLoading_GameState : GameStateBase
{
    [Inject] private readonly PlayerDataSaveSystem _playerDataSaveSystem;

    public override void OnEnter(object[] @params)
    {
        //Load data and other stuff..
        
        _playerDataSaveSystem.Initialize()
                             .GetAwaiter()
                             .OnCompleted(() => StateManager.ChangeState(GameState.Lobby));

        Event.OnGameLoadingStart?.Invoke();
    }
}

public class Lobby_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnEnterLobby?.Invoke();
    }
}

public class Playing_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnTableSessionStart?.Invoke();
    }
}

public class TableSessionEnd_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnTableSessionEnd?.Invoke();
    }
}