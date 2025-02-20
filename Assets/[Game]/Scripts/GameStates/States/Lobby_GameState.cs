public class Lobby_GameState : GameStateBase
{
    public override void OnEnter(object[] @params)
    {
        Event.OnEnterLobby?.Invoke();
    }
}