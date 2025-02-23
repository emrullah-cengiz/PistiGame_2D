using Cysharp.Threading.Tasks;
using VContainer;

public class TablePreparation_TableSessionState : TableSessionStateBase
{
    // ReSharper disable once AsyncVoidMethod
    public override async void OnEnter(object[] @params)
    {
        _tableSession.Setup(_tableData);
        
        await UniTask.WaitForSeconds(_tableSessionSettings.WaitDurationBeforeStartDiscards);
        
        ChangeState(TableSessionState.SessionStart);
    }
}