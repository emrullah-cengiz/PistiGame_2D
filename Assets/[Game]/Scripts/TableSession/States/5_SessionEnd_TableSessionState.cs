using Cysharp.Threading.Tasks;
using VContainer;

public class SessionEnd_TableSessionState : TableSessionStateBase
{
    public override async void OnEnter(object[] @params)
    {
        var result = _tableSession.EndGame();

        await UniTask.WaitForSeconds(_tableSessionSettings.WaitDurationBeforeResultPopup);

        await _saveSystem.SaveGameResult(result);

        Event.OnTableSessionGameEnd.Invoke(result);
        
        Event.OnBackToLobbyConfirmationBtn_Click += OnBackToLobbyConfirmationBtnClick;
    }

    private void OnBackToLobbyConfirmationBtnClick()
    {
        Event.OnBackToLobbyConfirmationBtn_Click -= OnBackToLobbyConfirmationBtnClick;

        Event.OnTableSessionEnd?.Invoke();
        // View.gameObject.SetActive(false);
    }
}