using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class BotPlayer : TablePlayer
{
    // private BotPlayerAI _behaviourTree;
    [Inject] private readonly TableSessionSettings _tableSessionSettings;
    [Inject] private readonly CardSettings _cardSettings;

    public override void Setup(int playerIndex, TablePlayerView view)
    {
        base.Setup(playerIndex, view);
        
        TablePlayerData = new TablePlayerData()
        {
            PlayerIndex = playerIndex,
            Name = "Player " + (playerIndex + 1),
        };
        
        Event.OnTablePlayerDataChanged?.Invoke(TablePlayerData);
    }

    public override async UniTask<CardData> PlayCard()
    {
        var cardToDiscard = GetBestCardToPlay();

        await UniTask.WaitForSeconds(_tableSessionSettings.BotWaitDurationBeforePlay);
        
        await Hand.TransferTo(_tableSession.DiscardPile, cardToDiscard, CardTransferOptions.Default);
        
        return cardToDiscard;
    }

    private CardData GetBestCardToPlay()
    {
        if (_tableSession.DiscardPile.HasAnyCard &&
            (Hand.HasAny(_tableSession.DiscardPile.LastAddedCard!.Value.Value, out var card) ||
             Hand.HasAnyJack(out card)))
            return card!.Value;

        return Hand.GetLowestSpecialOrAnyNonSpecialCard();
    }
}
