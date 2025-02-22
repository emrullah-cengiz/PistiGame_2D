using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TableSession_GameState : GameStateBase
{
    [Inject] private readonly GameLifetimeScope _lifetimeScope;
    [Inject] private readonly TableSessionLifetimeScopeInstaller _tableSessionLifetimeScopeInstaller;

    private LifetimeScope _tableSessionScope;

    public override void OnEnter(params object[] @params)
    {
        _tableSessionLifetimeScopeInstaller.SetScopeData((TableData)@params[0]);

        _tableSessionScope = _lifetimeScope.CreateChild(_tableSessionLifetimeScopeInstaller,
                                                        GlobalVariables.TABLE_SESSION_LIFE_TIME_SCOPE);

        //Sub state machine will start immediately after build 
        _tableSessionScope.Build();
        
        _tableSessionLifetimeScopeInstaller.InjectAdditionalTableObjects();
        
        Event.OnTableSessionEnd += OnTableSessionEnd;
    }

    private void OnTableSessionEnd()
    {
        Object.Destroy(_tableSessionScope);

        ChangeState(GameState.Lobby);
    }

    public override void OnExit() => Event.OnTableSessionEnd -= OnTableSessionEnd;
}