using Cysharp.Threading.Tasks;
using GAME.Utilities.StateMachine;

public class DealingCards_TableSessionState : TableSessionStateBase
{
    public async override void OnEnter(object[] @params)
    {
        base.OnEnter(@params);

        await _tableSession.DealCardsToPlayers();
        
        await UniTask.WaitForSeconds(_tableSessionSettings.WaitDurationBeforePlayLoopStart);
        
        ChangeState(TableSessionState.CardPlayingLoop);
    }
}
