public class TableSessionEnd_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnTableSessionEnd?.Invoke();
    }
}