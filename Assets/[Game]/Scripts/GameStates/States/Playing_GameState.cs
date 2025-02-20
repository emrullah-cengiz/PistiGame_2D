public class Playing_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnTableSessionStart?.Invoke();
    }
}