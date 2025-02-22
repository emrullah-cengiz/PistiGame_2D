public class SessionEnd_TableSessionState : TableSessionStateBase
{
    public override void OnEnter(object[] @params)
    {
        _tableSession.GetTableSessionUserResult();
    }
}