using Cysharp.Threading.Tasks;
using GAME.Utilities.StateMachine;
using UnityEngine;
using VContainer;

public class CardPlayingLoop_TableSessionState : TableSessionStateBase
{
    public async override void OnEnter(object[] @params)
    {
        base.OnEnter(@params);

        await _tableSession.ProcessTurnLoopsUntilHandsEmpty();

        ChangeState(_tableSession.DrawPile.HasAnyCard ? 
                        TableSessionState.DealingCards : 
                        TableSessionState.SessionEnd);
    }
}