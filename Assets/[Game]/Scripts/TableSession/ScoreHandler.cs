using System;
using System.Linq;
using VContainer;

public class ScoreHandler
{
    [Inject] private readonly TableSessionSettings _tableSessionSettings;
    private TableSession _tableSession;

    public void Setup(TableSession tableSession)
    {
        _tableSession = tableSession;
    }
    
    public void AddScoreToPlayerByDiscardedCard(int totalPilePoints, TablePlayer player, CardData lastDiscardedCard, CardData newDiscarded)
    {
        ScoreActionType scoreAction = default;

        player.CurrentScore += totalPilePoints;

        if (_tableSession.DiscardPile.Cards.Count == 1)
            if (newDiscarded.Value == CardValue.Jack)
                scoreAction = ScoreActionType.JackPisti;
            else if (lastDiscardedCard.Value == newDiscarded.Value)
                scoreAction = ScoreActionType.Pisti;
            else
                return;

        player.CurrentScore += _tableSessionSettings.ScoreActionPoints[scoreAction];
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddScoreByCollectedCardsMajority(TablePlayer winner)
    {
        TablePlayer mostCardsPlayer = null;
        TablePlayer secondMostCardsPlayer = null;

        foreach (var player in _tableSession.Players)
        {
            int cardCount = player.CollectedPile.Cards.Count;

            if (mostCardsPlayer == null || cardCount > mostCardsPlayer.CollectedPile.Cards.Count)
            {
                secondMostCardsPlayer = mostCardsPlayer;
                mostCardsPlayer = player;
            }
            else if (secondMostCardsPlayer == null || cardCount > secondMostCardsPlayer.CollectedPile.Cards.Count)
                secondMostCardsPlayer = player;
        }

        int mostCount = mostCardsPlayer.CollectedPile.Cards.Count;
        int secMostCount = secondMostCardsPlayer.CollectedPile.Cards.Count;

        var collectedCardNumberBasedPointAmount = _tableSessionSettings.ScoreActionPoints[ScoreActionType.CollectedCardsNumberMajority];
        
        if (mostCount > secMostCount)
            mostCardsPlayer.CurrentScore += collectedCardNumberBasedPointAmount;
        else if (mostCount == secMostCount)
            winner.CurrentScore += collectedCardNumberBasedPointAmount;
    }

    public TablePlayer GetWinner() =>
        _tableSession.Players.OrderByDescending(x => x.CurrentScore)
                     .FirstOrDefault();
}