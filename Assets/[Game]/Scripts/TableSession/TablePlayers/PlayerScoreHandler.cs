using System;
using VContainer;

public class PlayerScoreHandler
{
    [Inject] private readonly TableSession _tableSession;
    [Inject] private readonly TableSessionSettings _tableSessionSettings;

    public int CurrentScore { get; private set; }

    public void AddScoreByCard(int totalPilePoints, CardData lastDiscardedCard, CardData newDiscarded)
    {
        ScoreActionType scoreAction = default;

        CurrentScore += totalPilePoints;

        if (_tableSession.DiscardPile.Cards.Count == 1)
            if (newDiscarded.Value == CardValue.Jack)
                scoreAction = ScoreActionType.JackPisti;
            else if (lastDiscardedCard.Value == newDiscarded.Value)
                scoreAction = ScoreActionType.Pisti;
            else
                return;

        CurrentScore += _tableSessionSettings.ScoreActionPoints[scoreAction];
    }
}