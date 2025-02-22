using GAME.Utilities.StateMachine;
using UnityEngine;
using VContainer;

public class CardPlayingLoop_TableSessionState : TableSessionStateBase
{
    public async override void OnEnter(object[] @params)
    {
        await _tableSession.ExecuteTurnLoopsUntilCardsExhausted();
        
        ChangeState(TableSessionState.SessionEnd);
    }
}
