using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public class PlayerDataSaveSystem
{
    private PlayerData PlayerData;
    private PlayerData _playerDataCopy;

    private readonly DataSaveHandler<PlayerData> _playerDataHandler = new(GlobalVariables.PLAYER_DATA_FILE_NAME);

    public async UniTask Initialize()
    {
        PlayerData = await _playerDataHandler.Load();
        
        Event.OnPlayerDataLoaded?.Invoke();
    }

    public void IncreaseWinCount() => PlayerData.WinCount++;
    
    public void IncreaseLostCount() => PlayerData.LostCount++;
    public void AddMoney(int amount) => PlayerData.AccountBalance += amount;

    public PlayerData Get()
    {
        _playerDataCopy ??= PlayerData;

        _playerDataCopy.AccountBalance = PlayerData.AccountBalance;
        _playerDataCopy.LostCount = PlayerData.LostCount;
        _playerDataCopy.WinCount = PlayerData.WinCount;

        return _playerDataCopy;
    }
}