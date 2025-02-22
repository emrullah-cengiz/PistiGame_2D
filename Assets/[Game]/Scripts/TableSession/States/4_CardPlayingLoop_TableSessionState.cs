using GAME.Utilities.StateMachine;
using UnityEngine;
using VContainer;

public class CardPlayingLoop_TableSessionState : TableSessionStateBase
{
    [Inject] private readonly TablePlayLoopSubStateManager _subStateManager;
    public async override void OnEnter(object[] @params)
    {
        _subStateManager.Initialize();
    }
}
