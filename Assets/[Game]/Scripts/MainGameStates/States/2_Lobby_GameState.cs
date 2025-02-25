using UnityEngine;

public class Lobby_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnEnterLobby?.Invoke();

        Event.OnCreateTableButton_Click += GoToTableSessionState;
    }

    private void GoToTableSessionState(TableData tableData) =>
        ChangeState(GameState.TableSession, tableData);

    public override void OnExit() =>
        Event.OnCreateTableButton_Click -= GoToTableSessionState;
}