using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerDataSaveSystem
{
    private PlayerData _playerData;

    public PlayerData Data
    {
        get => _playerData;
        private set => _playerData = value;
    }

    private readonly DataSaveHandler<PlayerData> _playerDataHandler = new(Path.Combine(Application.dataPath, GlobalVariables.PLAYER_DATA_FILE_NAME));

    #region Injects

    [Inject] private readonly PlayerSettings _playerSettings;

    #endregion

    public async UniTask Initialize()
    {
        await Load();

        Event.OnPlayerDataLoaded?.Invoke();
    }

    public void IncreaseWinCount() => _playerData.WinCount++;
    public void IncreaseLostCount() => _playerData.LostCount++;
    public void AddMoney(int amount) => _playerData.AccountBalance += amount;

    private async UniTask Load()
    {
        var res = await _playerDataHandler.Load();
        _playerData = res.status ? res.data : GetPlayerStartData();
    }

    private async UniTask Save() => await _playerDataHandler.Save(_playerData);

    private PlayerData GetPlayerStartData() =>
        new()
        {
            Name = "Player",
            AccountBalance = _playerSettings.PlayerStartMoney
        };

    public async UniTask SaveGameResult(TableSessionResultData result)
    {
        if (result.IsWon)
        {
            IncreaseWinCount();
            AddMoney(result.EarnedMoney);
        }
        else
            IncreaseLostCount();

        await Save();
    }
}