using Cysharp.Threading.Tasks;

public class UserPlayer : TablePlayer
{
    public override bool IsUser => true;
    
    private UniTaskCompletionSource<CardData> _cardSelectionTask;
    
    public override async UniTask<CardData> PlayCard()
    {
        _cardSelectionTask = new UniTaskCompletionSource<CardData>();

        Event.OnCardSelected += OnCardSelected;
        
        var cardToDiscard = await _cardSelectionTask.Task;
        
        await Hand.TransferTo(_tableSession.DiscardPile, cardToDiscard, CardTransferOptions.Default);

        return cardToDiscard;
    }

    private void OnCardSelected(CardData card)
    {
        Event.OnCardSelected -= OnCardSelected;
        
        _cardSelectionTask.TrySetResult(card);
    }
}