using VContainer;

public class GameLoading_GameState : GameStateBase
{
    [Inject] private readonly PlayerDataSaveSystem _saveSystem;

    public override void OnEnter(object[] @params)
    {
        //Load data and other stuff..

        _saveSystem.Initialize()
                   .GetAwaiter()
                   .OnCompleted(() => ChangeState(GameState.Lobby));

        Event.OnGameLoadingStart?.Invoke();
    }
}