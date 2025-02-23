using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GAME.Utilities.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public enum TableSessionState
{
    TablePreparation,
    SessionStart,
    DealingCards,
    CardPlayingLoop,
    SessionEnd
}

public class TableSessionStateManager : IInitializable
{
    private StateMachine<TableSessionState> _stateMachine;

    [Inject] private TablePreparation_TableSessionState _tablePreparationTableSessionState;
    [Inject] private SessionStart_TableSessionState _sessionStartTableSessionState;
    [Inject] private DealingCards_TableSessionState _dealingCardsTableSessionState;
    [Inject] private CardPlayingLoop_TableSessionState _cardPlayingLoopTableSessionState;
    [Inject] private SessionEnd_TableSessionState _sessionEndTableSessionState;

    public void Initialize()
    {
        Debug.Log($"{nameof(TableSessionStateManager)}::Initialize");

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new StateMachine<TableSessionState>();

        _stateMachine.OnStateChanged += OnStateChanged;

        _stateMachine.AddState(TableSessionState.TablePreparation, _tablePreparationTableSessionState);
        _stateMachine.AddState(TableSessionState.SessionStart, _sessionStartTableSessionState);
        _stateMachine.AddState(TableSessionState.DealingCards, _dealingCardsTableSessionState);
        _stateMachine.AddState(TableSessionState.CardPlayingLoop, _cardPlayingLoopTableSessionState);
        _stateMachine.AddState(TableSessionState.SessionEnd, _sessionEndTableSessionState);

        _stateMachine.SetStartState(TableSessionState.TablePreparation);
        _stateMachine.Init();
    }

    private void OnStateChanged(TableSessionState state) => 
        Debug.Log($"<color=blue>Table Session State</color> changing.. {_stateMachine.CurrentState} > {state}");
}

public abstract class TableSessionStateBase : StateBase<TableSessionState>
{
    [Inject] protected readonly TableSessionSettings _tableSessionSettings;
    [Inject] protected readonly TableSession _tableSession;
    [Inject] protected readonly TableData _tableData;
}