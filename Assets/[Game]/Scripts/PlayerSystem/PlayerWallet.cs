using VContainer;

public class PlayerWallet
{
    [Inject] private readonly PlayerDataSaveSystem _saveSystem;

    public bool HasEnoughBalance(int amount) => _saveSystem.Data.AccountBalance >= amount;
    public void IncreaseBalance(int amount) => _saveSystem.AddMoney(amount);
    public bool TryDecreaseBalance(int amount)
    {
        if (!HasEnoughBalance(amount))
            return false;

        _saveSystem.AddMoney(-amount);
        
        return true;
    }
}