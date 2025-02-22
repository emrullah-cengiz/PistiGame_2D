using Cysharp.Threading.Tasks;

public class SessionStart_TableSessionState : TableSessionStateBase
{
    // ReSharper disable once AsyncVoidMethod
    public override async void OnEnter(object[] @params)
    {
        await _tableSession.DiscardFirstCards();

        await UniTask.WaitForSeconds(_tableSessionSettings.WaitDurationBeforeCardDealing);
        
        ChangeState(TableSessionState.DealingCards);
    }
}