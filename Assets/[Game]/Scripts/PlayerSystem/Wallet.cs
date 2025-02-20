using VContainer;

public class Wallet
{
    [Inject] private readonly PlayerDataSaveSystem _dataSaveSystem;

    public bool HasEnoughBalance(int amount)
    {
        return _dataSaveSystem.Get().AccountBalance >= amount;
    }

    public void IncreaseBalance(int amount) => _dataSaveSystem.AddMoney(amount);

    public bool TryDecreaseBalance(int amount)
    {
        if (!HasEnoughBalance(amount))
            return false;

        _dataSaveSystem.AddMoney(-amount);
        
        return true;
    }
}