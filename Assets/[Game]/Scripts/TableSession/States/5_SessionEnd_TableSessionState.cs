public class SessionEnd_TableSessionState : TableSessionStateBase
{
    public override void OnEnter(object[] @params)
    {
        var r = _tableSession.EndGame();

        Event.OnTableSessionEnd.Invoke(r);
    }
}