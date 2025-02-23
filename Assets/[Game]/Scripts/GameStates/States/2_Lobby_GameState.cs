public class Lobby_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnEnterLobby?.Invoke();
        
        Event.OnTableCreateButton_Click += ChangeToTableSessionState;
    }

    private void ChangeToTableSessionState(TableData tableData) => 
        ChangeState(GameState.TableSession, tableData);

    public override void OnExit() => 
        Event.OnTableCreateButton_Click -= ChangeToTableSessionState;
}