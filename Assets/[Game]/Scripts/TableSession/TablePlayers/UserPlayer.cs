using Cysharp.Threading.Tasks;
using VContainer;

public class UserPlayer : TablePlayer
{
    [Inject] private readonly PlayerDataSaveSystem _playerDataSaveSystem;
    public override bool IsUser => true;

    private UniTaskCompletionSource<CardData> _cardSelectionTask;

    public override void Setup(int playerIndex, TablePlayerView view)
    {
        base.Setup(playerIndex, view);

        TablePlayerData = new TablePlayerData()
        {
            PlayerIndex = playerIndex,
            Name = _playerDataSaveSystem.Data.Name,
        };

        Event.OnTablePlayerDataChanged?.Invoke(TablePlayerData);
    }

    public override async UniTask<CardData> PlayCard()
    {
        _cardSelectionTask = new UniTaskCompletionSource<CardData>();
        Event.OnCardSelected += OnCardSelected;

        Hand.SetCardsInteractable(true);

        var cardToDiscard = await _cardSelectionTask.Task;

        Hand.SetCardsInteractable(false);

        await Hand.TransferTo(_tableSession.DiscardPile, cardToDiscard, CardTransferOptions.Default);

        return cardToDiscard;
    }

    private void OnCardSelected(CardData card)
    {
        Event.OnCardSelected -= OnCardSelected;

        _cardSelectionTask.TrySetResult(card);
    }
}