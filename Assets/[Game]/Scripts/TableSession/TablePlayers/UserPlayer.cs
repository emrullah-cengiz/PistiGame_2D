using Cysharp.Threading.Tasks;

public class UserPlayer : TablePlayerBase
{
    private UniTaskCompletionSource<CardData> _cardSelectionTask;
    public override async UniTask<CardData> PlayCard()
    {
        _cardSelectionTask = new UniTaskCompletionSource<CardData>();

        Event.OnCardSelected += OnCardSelected;
        
        return await _cardSelectionTask.Task;
    }

    private void OnCardSelected(CardData card)
    {
        Event.OnCardSelected -= OnCardSelected;
        
        _cardSelectionTask.TrySetResult(card);
    }
}