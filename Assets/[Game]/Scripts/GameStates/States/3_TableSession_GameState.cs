using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TableSession_GameState : GameStateBase
{
    [Inject] private readonly PlayerWallet _playerWallet;
    
    [Inject] private readonly GameLifetimeScope _lifetimeScope;
    [Inject] private readonly TableSessionLifetimeScopeInstaller _tableSessionLifetimeScopeInstaller;

    private LifetimeScope _tableSessionScope;

    public override void OnEnter(params object[] @params)
    {
        Event.OnTableSessionStart?.Invoke();
        
        var data = (TableData)@params[0];

        //charge bet amount
        if(!_playerWallet.TryDecreaseBalance(data.BetAmount))
            return;
        
        BuildTableSessionLifetimeScope(data);

        Event.OnTableSessionEnd += OnTableSessionEnd;
    }

    private void OnTableSessionEnd(TableSessionUserResultData result)
    {
        Object.Destroy(_tableSessionScope);

        ChangeState(GameState.Lobby);
    }

    public override void OnExit() => Event.OnTableSessionEnd -= OnTableSessionEnd;

    private void BuildTableSessionLifetimeScope(TableData data)
    {
        _tableSessionLifetimeScopeInstaller.SetScopeData(data);

        _tableSessionScope = _lifetimeScope.CreateChild(_tableSessionLifetimeScopeInstaller,
                                                        GlobalVariables.TABLE_SESSION_LIFE_TIME_SCOPE);

        //Sub state machine will start immediately after build 
        _tableSessionScope.Build();
    }
}